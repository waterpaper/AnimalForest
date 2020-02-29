<?php
//unity import
$userID = $_POST['ID'];

//mysql의 설정을 입력합니다
$master = mysqli_connect("localhost","root","rndtn123","data") or ("error");

if(!$master)
	die("error");

mysqli_select_db($master, "data") or die("error");

//users라는 테이블에서 받은id와 같은 id를 찾겟다는 의미입니다.
$sqlCheckQuery = "SELECT * FROM statement WHERE id = '".$userID."';";
$sqlItemCheckQuery = "SELECT * FROM item WHERE UserID = '".$userID."';";
$sqlQuestCheckQuery = "SELECT * FROM quest WHERE UserID = '".$userID."';";

$check = mysqli_query($master,$sqlCheckQuery) or Die("error");

if(mysqli_num_rows($check)==0)
{
	die("NotExisted");
}
else
{
	$row = mysqli_fetch_assoc($check) or Die("error");
	$dataArray = array(
		"ID" => $row['id'],
		"Name" => $row['Name'],
		"Kind" => (int)$row['Kind'],
		"Money" => (int)$row['Money'],
		"Level" => (int)$row['Level'],
		"Exp" => (int)$row['Exp'],
		"Hp" => (float)$row['Hp'],
		"HpMax" => (float)$row['HpMax'],
		"Mp" => (float)$row['MpMax'],
		"MpMax" => (float)$row['MpMax'],
		"Atk" => (float)$row['Atk'],
		"Def" => (float)$row['Def'],
		"MapNumber" =>(int)$row['MapNumber'],
		"EquipWeaponItem" =>(int)$row['EquipWeaponItem'],
		"EquipArmorItem" =>(int)$row['EquipArmorItem'],
		"EquipShieldItem" =>(int)$row['EquipShieldItem'],
		"EquipHpPotion" =>(int)$row['EquipHpPotion'],
		"EquipMpPotion" =>(int)$row['EquipMpPotion'],
		"ClearQuest" =>$row['ClearQuest'],
		"ClearEvent" =>$row['ClearEvent']
	);
	
	$positionArray = array(
		"x" =>(float)$row['MapPositionX'],
		"y" =>(float)$row['MapPositionY'],
		"z" =>(float)$row['MapPositionZ'],
		
	);
	
	$dataArray['MapPosition'] = $positionArray;
	
		
	//아이템 추가 구문입니다.
	$itemCheck = mysqli_query($master,$sqlItemCheckQuery) or Die("error");
	$itemArray = array();
		
	if(mysqli_num_rows($itemCheck)==0)
	{
		$dataArray['ItemList'] = $itemArray;
	}
	else
	{
		foreach($itemCheck as $itemRow)
		{
			$itemIndex = array(
				"ItemID" =>(int)$itemRow['ItemID'],
				"ItemCount" =>(int)$itemRow['ItemCount'],
				"InventoryNum" =>(int)$itemRow['InventoryNumber']
			);
			array_push($itemArray, $itemIndex);
		}
		$dataArray['ItemList'] = $itemArray;
	}
		
	//퀘스트 추가 구문입니다.
	$questCheck = mysqli_query($master,$sqlQuestCheckQuery) or Die("error");
	$questArray = array();
		
	if(mysqli_num_rows($questCheck)==0)
	{
		$dataArray['QuestList'] = $questArray;
	}
	else
	{
		foreach($questCheck as $questRow)
		{
			$questIndex = array(
				"QuestID" =>(int)$questRow['QuestID'],
				"TargetNowCount" =>(float)$questRow['TargetCount']
			);
			array_push($questArray, $questIndex);
		}
		$dataArray['QuestList'] = $questArray;
	}
}
	
$jsonData = json_encode($dataArray);
	
Die($jsonData) or Die("error");
?>