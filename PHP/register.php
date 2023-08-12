<?php
session_start();

//Check if the server is logged in
if (!isset($_SESSION["server_id"])) {
    echo "Server not logged in";
    exit(); //Stop further execution of the script
}

//Retrieve user data from the URL
$email = $_POST['email'];
$password = $_POST['password'];
$nickname = $_POST['nickname'];

//Validate and sanitize the input data
$email = filter_var($email, FILTER_SANITIZE_EMAIL);
$password = filter_var($password, FILTER_SANITIZE_STRING);
$nickname = filter_var($nickname, FILTER_SANITIZE_STRING);

//Validate email format
if (!filter_var($email, FILTER_VALIDATE_EMAIL)) {
    echo "Invalid email format";
    exit();
}

//Perform database operations to insert the user into the users table
$servername = "localhost";
$username = "bradleyvanewijk";
$dbPassword = "Ohwu9chuth5r";
$dbname = "bradleyvanewijk";

//Create a new connection to the database
$conn = new mysqli($servername, $username, $dbPassword, $dbname);

//Check the connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

//Check if the email already exists in the database
$stmt = $conn->prepare("SELECT * FROM users WHERE email = ?");
$stmt->bind_param("s", $email);
$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows > 0) {
    //Email already exists, return an error message
    echo "Email already exists";
} else {
    //Email does not exist, insert the user into the database
    $hashed_password = password_hash($password, PASSWORD_DEFAULT);
    $insertStmt = $conn->prepare("INSERT INTO users (email, password, nickname) VALUES (?, ?, ?)");
    $insertStmt->bind_param("sss", $email, $hashed_password, $nickname);

    if ($insertStmt->execute()) {
        //User insertion successful
        echo "User registered successfully";
    } else {
        //User insertion failed
        echo "User registration failed";
    }

    $insertStmt->close();
}

$stmt->close();
$conn->close();
?>