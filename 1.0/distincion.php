<?php
$arch = fopen($_POST['archivo'].'txt','w');
$lat = $_POST['latitud'];
$long = $_POST['longitud'];
settype($lat,'float');
settype($long,'float');
if ($lat ==1.00 && $long ==1.00){
	$t = 'Zona1';
} elseif($lat ==2.00 && $long ==2.00) {
	$t = 'Zona2';
} elseif($lat ==3.00 && $long ==3.00) {
	$t = 'Zona3';
} elseif($lat ==4.00 && $long ==4.00) {
	$t = 'Zona4';
} elseif($lat ==5.00 && $long ==5.00) {
	$t = 'Zona5';
}
fwrite($arch, $t);
fclose($arch);
echo 'ok';
