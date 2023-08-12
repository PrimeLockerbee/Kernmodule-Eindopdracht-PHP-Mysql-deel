<?php
session_start();

// Perform database operations to retrieve highscores
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

// Query to retrieve highscores from highest to lowest
$query = "
    SELECT u.nickname, s.score
    FROM users u
    JOIN scores s ON u.id = s.user_id
    ORDER BY s.score DESC";

$result = $conn->query($query);

if ($result) {
    $highscores = [];
    while ($row = $result->fetch_assoc()) {
        $highscores[] = $row;
    }

    // Convert the highscores data to JSON format
    $jsonResponse = json_encode($highscores, JSON_PRETTY_PRINT);
    echo $jsonResponse;
} else {
    echo "Error retrieving highscores";
}

$conn->close();
?>






