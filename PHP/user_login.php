<?php
session_start();

// Check if the server is logged in
if (!isset($_SESSION["server_id"])) {
    echo "Server not logged in";
    exit(); // Stop further execution of the script
}

// Retrieve the email and password from the POST request
$email = $_POST['email'];
$password = $_POST['password'];

// Validate and sanitize the input data (you can add your own validation logic here)

// Perform database operations for registration and login
$servername = "localhost";
$username = "bradleyvanewijk";
$dbPassword = "Ohwu9chuth5r";
$dbname = "bradleyvanewijk";

// Create a new connection to the database
$conn = new mysqli($servername, $username, $dbPassword, $dbname);

// Check the connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Registration: Hash the provided password before storing it in the database
if (isset($_POST['registration'])) {
    $hashed_password = password_hash($password, PASSWORD_DEFAULT);

    // Insert user data into the database
    $insertStmt = $conn->prepare("INSERT INTO users (email, password, nickname) VALUES (?, ?, ?)");
    $insertStmt->bind_param("sss", $email, $hashed_password, $nickname);

    if ($insertStmt->execute()) {
        echo "User registered successfully";
    } else {
        echo "User registration failed";
    }

    $insertStmt->close();
} else { // Login: Verify hashed password
    $stmt = $conn->prepare("SELECT password, nickname, id FROM users WHERE email = ?");
    $stmt->bind_param("s", $email);
    $stmt->execute();
    $stmt->store_result();

    if ($stmt->num_rows > 0) {
        $stmt->bind_result($storedPassword, $nickname, $id);
        $stmt->fetch();

        // Verify the provided password against the stored hashed password
        if (password_verify($password, $storedPassword)) {

            $_SESSION["player_id"] = $id;
            $_SESSION["player_nickname"] = $nickname;

            echo $id . "\n" . $nickname . "\n" . session_id();
        } else {
            // Password verification failed, login is invalid
            echo "Invalid email or password";
        }
    } else {
        // No matching email found in the database
        echo "Invalid email or password";
    }
}

// Close the statement and the connection
$stmt->close();
$conn->close();
?>