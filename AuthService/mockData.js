const bcrypt = require("bcryptjs");

const users = [
  {
    id: 1,
    username: "admin",
    password: bcrypt.hashSync("admin123", 10),
  },
  {
    id: 2,
    username: "user",
    password: bcrypt.hashSync("user123", 10),
  },
];

module.exports = { users };
