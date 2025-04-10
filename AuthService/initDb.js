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
          role NVARCHAR(50) NOT NULL
        );

        INSERT INTO Users (username, password, role)
        VALUES 
          ('jane', '${bcrypt.hashSync("janepass", 10)}', 'user'),
          ('bob', '${bcrypt.hashSync("bobsecure", 10)}', 'user'),
          ('sara', '${bcrypt.hashSync("sarapass", 10)}', 'user'),
          ('tom', '${bcrypt.hashSync("tomcat", 10)}', 'user'),
          ('lisa', '${bcrypt.hashSync("lisapwd", 10)}', 'user'),
          ('mark', '${bcrypt.hashSync("mark", 10)}', 'user'),
          ('jasper', '${bcrypt.hashSync("jasperadmin", 10)}', 'admin')
      END
    `);

    console.log("✅ DB + Users table ready");
  } catch (error) {
    console.error("❌ Error setting up DB", error);
  }
}

module.exports = { initDatabase, dbConfig };
