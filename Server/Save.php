<?php
//unity import
$userID = $_POST['ID'];
$json_string = $_POST['SaveData'];

//mysql의 설정을 입력합니다
$master = mysqli_connect("localhost","root","rndtn123","data") or ("Cannot connect!".mysqli_error());

if(!$master)
	die("Connect Error:".mysqli_error());

mysqli_select_db($master, "data") or die("could not load the database".mysqli_error());

$R = json_decode($json_string, true);

//users라는 테이블에서 받은id와 같은 id를 찾겟다는 의미입니다.
$sqlCheckQuery = "SELECT * FROM statement WHERE id = '".$userID."';";

$check = mysqli_query($master,$sqlCheckQuery) or Die("mysqli_checkQuery 오류");

if(mysqli_num_rows($check)==0)
{
	$sqlQuery = "INSERT INTO statement(id, Name, Kind, Money, Level, Exp, Hp, HpMax, Mp, MpMax, Atk, Def, MapNumber, MapPositionX, MapPositionY, MapPositionZ, EquipWeaponItem, EquipArmorItem, EquipShieldItem, EquipHpPotion, EquipMpPotion, ClearEvent, ClearQuest) 
	VALUES (";
	$sqlQuery = $sqlQuery."'".$R['ID']."',";
	$sqlQuery = $sqlQuery."'".$R['Name']."',";
	$sqlQuery = $sqlQuery."'".$R['Kind']."',";
	$sqlQuery = $sqlQuery."'".$R['Money']."',";
	$sqlQuery = $sqlQuery."'".$R['Level']."',";
	$sqlQuery = $sqlQuery."'".$R['Exp']."',";
	$sqlQuery = $sqlQuery."'".$R['Hp']."',";
	$sqlQuery = $sqlQuery."'".$R['HpMax']."',";
	$sqlQuery = $sqlQuery."'".$R['Mp']."',";
	$sqlQuery = $sqlQuery."'".$R['MpMax']."',";
	$sqlQuery = $sqlQuery."'".$R['Atk']."',";
	$sqlQuery = $sqlQuery."'".$R['Def']."',";
	$sqlQuery = $sqlQuery."'".$R['MapNumber']."',";
	$sqlQuery = $sqlQuery."'".$R['MapPosition']['x']."',";
	$sqlQuery = $sqlQuery."'".$R['MapPosition']['y']."',";
	$sqlQuery = $sqlQuery."'".$R['MapPosition']['z']."',";
	$sqlQuery = $sqlQuery."'".$R['EquipWeaponItem']."',";
	$sqlQuery = $sqlQuery."'".$R['EquipArmorItem']."',";
	$sqlQuery = $sqlQuery."'".$R['EquipShieldItem']."',";
	$sqlQuery = $sqlQuery."'".$R['EquipHpPotion']."',";
	$sqlQuery = $sqlQuery."'".$R['EquipMpPotion']."',";
	$sqlQuery = $sqlQuery."'".$R['ClearQuest']."',";
	$sqlQuery = $sqlQuery."'".$R['ClearEvent']."')";
	
	$saves = mysqli_query($master,$sqlQuery) or Die("mysqli_query 오류"."'.$sqlQuery'");
}
else
{
	$sqlQuery = "UPDATE statement SET ";
	
	$sqlQuery = $sqlQuery."id ='".$R['ID']."',";
	$sqlQuery = $sqlQuery."Name ='".$R['Name']."',";
	$sqlQuery = $sqlQuery."Kind ='".$R['Kind']."',";
	$sqlQuery = $sqlQuery."Money ='".$R['Money']."',";
	$sqlQuery = $sqlQuery."Level ='".$R['Level']."',";
	$sqlQuery = $sqlQuery."Exp ='".$R['Exp']."',";
	$sqlQuery = $sqlQuery."Hp ='".$R['Hp']."',";
	$sqlQuery = $sqlQuery."HpMax ='".$R['HpMax']."',";
	$sqlQuery = $sqlQuery."Mp ='".$R['Mp']."',";
	$sqlQuery = $sqlQuery."MpMax ='".$R['MpMax']."',";
	$sqlQuery = $sqlQuery."Atk ='".$R['Atk']."',";
	$sqlQuery = $sqlQuery."Def ='".$R['Def']."',";
	$sqlQuery = $sqlQuery."MapNumber ='".$R['MapNumber']."',";
	$sqlQuery = $sqlQuery."MapPositionX ='".$R['MapPosition']['x']."',";
	$sqlQuery = $sqlQuery."MapPositionY ='".$R['MapPosition']['y']."',";
	$sqlQuery = $sqlQuery."MapPositionZ ='".$R['MapPosition']['z']."',";
	$sqlQuery = $sqlQuery."EquipWeaponItem ='".$R['EquipWeaponItem']."',";
	$sqlQuery = $sqlQuery."EquipArmorItem ='".$R['EquipArmorItem']."',";
	$sqlQuery = $sqlQuery."EquipShieldItem ='".$R['EquipShieldItem']."',";
	$sqlQuery = $sqlQuery."EquipHpPotion ='".$R['EquipHpPotion']."',";
	$sqlQuery = $sqlQuery."EquipMpPotion ='".$R['EquipMpPotion']."',";
	$sqlQuery = $sqlQuery."ClearQuest ='".$R['ClearQuest']."',";
	$sqlQuery = $sqlQuery."ClearEvent ='".$R['ClearEvent']."';";
	
	$saves = mysqli_query($master,$sqlQuery) or Die("mysqli_query 오류"."'.$sqlQuery'");


	//아이템 저장 구문입니다. 인벤토리 모든 아이템을 삭제후 삽입해줍니다.
	$sqlItemQuery ="DELETE FROM item WHERE UserID = '".$userID."';";
	$saves = mysqli_query($master,$sqlItemQuery) or Die("mysqli_itemSave 오류1"."'.$sqlItemQuery'");

	$sqlItemQuery ="INSERT INTO item(UserID, ItemID, ItemCount, InventoryNumber) 
	VALUES (";
	
	foreach($R['ItemList'] as $item)
	{
		$sqlItemIndexQuery=$sqlItemQuery."'".$userID."', '".$item['ItemID']."', '".$item['ItemCount']."', '".$item['InventoryNum']."');";
		$saves = mysqli_query($master,$sqlItemIndexQuery) or Die("mysqli_itemSave 오류2"."'.$sqlItemIndexQuery'");
	}
	
	//퀘스트 저장 구문입니다. 퀘스트 정보를 모두 삭제후 삽입해줍니다.
	$sqlQuestQuery ="DELETE FROM quest WHERE UserID = '".$userID."';";
	$saves = mysqli_query($master,$sqlQuestQuery) or Die("mysqli_qusetSave 오류1"."'.$sqlQuestQuery'");

	$sqlQuestQuery ="INSERT INTO quest(UserID, QuestID, TargetCount) 
	VALUES (";
	
	foreach($R['QuestList'] as $quset)
	{
		$sqlQusetIndexQuery=$sqlQuestQuery."'".$userID."', '".$quset['QuestID']."', '".$quset['TargetNowCount']."');";
		$saves = mysqli_query($master,$sqlQusetIndexQuery) or Die("mysqli_qusetSave 오류2"."'.$sqlQusetIndexQuery'");
	}
}

//mysqli_num_row는 데이터베이스 쿼리결과 레코드 갯수를 반환합니다.
$numrows = mysqli_num_rows($check);
if($numrows == 0)
{
	die("Error");
}
else
{
	die("OK");
}
?>