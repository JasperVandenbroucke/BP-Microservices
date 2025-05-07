const sql = require("mssql");
const bcrypt = require("bcryptjs");

const dbConfig = {
  user: process.env.DB_USER,
  password: process.env.DB_PASSWORD,
  server: process.env.DB_HOST,
  database: process.env.DB_NAME,
  options: {
    encrypt: false,
    trustServerCertificate: true,
  },
};

const baseConfig = {
  user: process.env.DB_USER,
  password: process.env.DB_PASSWORD,
  server: process.env.DB_HOST,
  database: process.env.DB_MASTER,
  options: {
    encrypt: false,
    trustServerCertificate: true,
  },
};

async function initDatabase() {
  try {
    // 1. Verbinden met master
    const masterPool = await sql.connect(baseConfig);

    // 2. Maak database aan indien nodig
    await masterPool.request().query(`
      IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '${process.env.DB_NAME}')
      BEGIN
        CREATE DATABASE [${process.env.DB_NAME}];
      END
    `);

    // 3. Verbinden met de aangemaakte database
    await sql.close();
    const dbPool = await sql.connect(dbConfig);
    console.log(`--> Using DB: ${dbPool.config.database}`);

    // 4. Maak Users-tabel aan indien nodig
    await dbPool.request().query(`
      IF NOT EXISTS (
        SELECT * FROM INFORMATION_SCHEMA.TABLES 
        WHERE TABLE_NAME = 'Users'
      )
      BEGIN
        CREATE TABLE Users (
          id INT PRIMARY KEY IDENTITY(1,1),
          username NVARCHAR(255) UNIQUE NOT NULL,
          password NVARCHAR(255) NOT NULL,
        );
      END
    `);

    const users = [
      { username: "jane", password: "janepass" },
      { username: "bob", password: "bobsecure" },
      { username: "sara", password: "sarapass" },
      { username: "tom", password: "tomcat" },
      { username: "lisa", password: "lisapwd" },
      { username: "mark", password: "mark" },
      { username: "jasper", password: "jasperadmin" },
    ];

    for (const user of users) {
      const hashedPwd = await bcrypt.hash(user.password, 10);

      // Alleen toevoegen als gebruiker nog niet bestaat
      const exists = await dbPool
        .request()
        .input("username", sql.NVarChar, user.username)
        .query("SELECT 1 FROM Users WHERE username = @username");

      if (exists.recordset.length === 0) {
        await dbPool
          .request()
          .input("username", sql.NVarChar, user.username)
          .input("password", sql.NVarChar, hashedPwd).query(`
            INSERT INTO Users (username, password)
            VALUES (@username, @password)
          `);
      }
    }

    console.log("✅ DB + Users table ready");
  } catch (error) {
    console.error("❌ Error setting up DB", error);
  }
}

module.exports = { initDatabase, dbConfig };
