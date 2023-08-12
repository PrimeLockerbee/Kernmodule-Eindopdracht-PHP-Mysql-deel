<?php
session_start();

// Set server ID
$serverId = 1;

// Check if the server is logged in
if (!isset($_SESSION["server_id"])) {
    echo "0"; // Server not logged in
    exit();
}

// Retrieve the user ID and score from the POST request
$userId = filter_input(INPUT_POST, 'user_id', FILTER_VALIDATE_INT);
$score = filter_input(INPUT_POST, 'score', FILTER_VALIDATE_INT);

// Validate input data
if ($userId === false || $score === false || $userId === null || $score === null) {
    echo "Invalid input data";
    exit();
}

// Perform database operations to store the score
$servername = "localhost";
$username = "bradleyvanewijk";
$dbPassword = "Ohwu9chuth5r";
$dbname = "bradleyvanewijk";

// Create a new connection to the database
$conn = new mysqli($servername, $username, $dbPassword, $dbname);

// Check the connection
if ($conn->connect_error) {
    echo "0"; // Connection failed
    exit();
}

// Check if the player already has a score record in the scores table
$scoreCheckStmt = $conn->prepare("SELECT * FROM scores WHERE user_id = ?");
$scoreCheckStmt->bind_param("i", $userId);
$scoreCheckStmt->execute();
$scoreCheckResult = $scoreCheckStmt->get_result();

if ($scoreCheckResult->num_rows > 0) {
    // Player has a score record, update their score
    $updateStmt = $conn->prepare("UPDATE scores SET score = ?, server_id = ? WHERE user_id = ?");
    $updateStmt->bind_param("iii", $score, $serverId, $userId);

    if ($updateStmt->execute()) {
        // Score update successful
        echo "Score Update success"; // Success
    } else {
        // Score update failed
        echo "Score Update failed: " . $conn->error; // Failure
    }

    $updateStmt->close();
} else {
    // Player does not have a score record, insert a new score record
    $scoreInsertStmt = $conn->prepare("INSERT INTO scores (user_id, score, server_id, game_date) VALUES (?, ?, ?, NOW())");
    $scoreInsertStmt->bind_param("iii", $userId, $score, $serverId);

    if ($scoreInsertStmt->execute()) {
        // Score insertion successful
        echo "Score Insertion success"; // Success
    } else {
        // Score insertion failed
        echo "Score Insertion failed: " . $conn->error; // Failure
    }

    $scoreInsertStmt->close();
}

$scoreCheckStmt->close();
$conn->close();
?>
