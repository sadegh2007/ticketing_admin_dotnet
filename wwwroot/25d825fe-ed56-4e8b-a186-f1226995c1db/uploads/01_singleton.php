<?php

class Database {

    private static $instance;

    public static function getInstance()
    {
        if (!self::$instance) {
            self::$instance = new Database();
        }
        return self::$instance;
    }

    private function __construct()
    {
    }
}

$db = Database::getInstance();
$db2 = Database::getInstance();
$db3 = Database::getInstance();

var_dump($db);
var_dump($db2);
var_dump($db3);