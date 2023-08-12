<?php
session_start();

var_dump($_POST);

//Retrieve the server_id and password from the POST request
$serverid = $_POST['server_id'];
$password = $_POST['password'];

//Debug statements
echo "Server ID: " . $serverid . "<br>";
echo "Password: " . $password . "<br>";

//Validate and sanitize the input data (you can add your own validation logic here)

//Perform database operations to verify the server login data
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

//Prepare the SQL statement to retrieve the stored hashed password for the given server ID
$stmt = $conn->prepare("SELECT password FROM servers WHERE server_id = ?");
$stmt->bind_param("i", $serverid);
$stmt->execute();
$stmt->store_result();

//Check if a matching server ID is found in the database
if ($stmt->num_rows > 0) {
    $stmt->bind_result($storedPassword);
    $stmt->fetch();

    echo "Stored Password: " . $storedPassword . "<br>";

    //Verify the provided password against the stored password
    if ($password == $storedPassword) {
        //Password verification successful, generate a unique session ID
        session_regenerate_id(true); // Generate a new unique session ID

        // Store the server ID as a session variable
        $_SESSION["server_id"] = $serverid;

        // Set the session ID as a response header
        header('Session-ID: ' . session_id());

        // Return a success message
        echo "Server login successful";
    } else {
        //Password verification failed, return 0 (failure)
        echo "0";
    }
} else {
    //No matching server ID found in the database, return 0 (failure)
    echo "0";
}

//Close the statement and the connection
$stmt->close();
$conn->close();
?>