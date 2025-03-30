require("dotenv").config();
const express = require("express");
const jwt = require("jsonwebtoken");
const bcrypt = require("bcryptjs");
const cors = require("cors");

const app = express();
app.use(express.json());
app.use(cors());

const users = [
  {
    id: 1,
    username: "admin",
    password: bcrypt.hashSync("admin123", 10),
    role: "admin",
  },
  {
    id: 2,
    username: "user",
    password: bcrypt.hashSync("user123", 10),
    role: "user",
  },
];

const SECRET_KEY = process.env.JWT_SECRET;

app.post("/login", (req, res) => {
  console.log("--> Trying to login...");

  const { username, password } = req.body;
  const user = users.find((u) => u.username === username);
  if (!user) {
    console.log("--> ⛔ User not found");
    return res.status(401).json({ message: "User not found" });
  }

  if (!bcrypt.compareSync(password, user.password)) {
    console.log("--> ⛔ Invalid credentials");
    return res.status(401).json({ message: "Invalid credentials" });
  }

  const token = jwt.sign(
    { id: user.id, username: user.username, role: user.role },
    SECRET_KEY,
    { expiresIn: "3h" }
  );

  res.status(200).json({ token });
  console.log("--> ✅ Login successful!");
});

app.get("/validate", (req, res) => {
  console.log("--> Validating token...");

  const token = req.header("Authorization")?.split(" ")[1];
  if (!token) {
    console.log("--> ⛔ Access denied");
    return res.status(401).json({ message: "Access denied" });
  }

  try {
    const decodedUser = jwt.verify(token, SECRET_KEY);

    res.setHeader("UserId", decodedUser.id);
    res.setHeader("Username", decodedUser.username);

    res.json({ valid: true, user: decodedUser });
    console.log("--> ✅ Validation successful!");
  } catch (err) {
    console.log("--> ⛔ Invalid token");
    res.status(401).json({ message: "Invalid token" });
  }
});

const PORT = process.env.PORT;
app.listen(PORT, () => console.log(`🚀 AuthService running on port ${PORT}`));
