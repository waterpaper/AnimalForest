<?php
//unity import
$userID = $_POST['NewID'];
$userPassward = $_POST['NewPassward'];

//mysql의 설정을 입력합니다
$master = mysqli_connect("localhost","root","rndtn123","data") or ("Cannot connect!".mysqli_error());

if(!$master)
	die("Connect Error:".mysqli_error());

mysqli_select_db($master, "data") or die("could not load the database".mysqli_error());

//users라는 테이블에서 받은id와 같은 id를 찾겟다는 의미입니다.
$sqlQuery = "SELECT * FROM users WHERE id = '".$userID."';";

$check = mysqli_query($master,$sqlQuery) or Die("mysqli_query 오류");

//mysqli_num_row는 데이터베이스 쿼리결과 레코드 갯수를 반환합니다.
$numrows = mysqli_num_rows($check);
if($numrows == 0)
{
	$Result = mysqli_query($master, "INSERT INTO users(id, passward) VALUES ('".$userID."', '".$userPassward."');");
	
	if($Result)
		die("Create");
	else
		die("Error");
}
else
{
	die("Exist");
}

?>