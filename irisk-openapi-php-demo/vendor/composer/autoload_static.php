<?php

// autoload_static.php @generated by Composer

namespace Composer\Autoload;

class ComposerStaticInit2619668dffef626b1919ad6e94bcd33e
{
    public static $files = array (
        '9a20f157b7f8081f7b27dc8b61425e9e' => __DIR__ . '/..' . '/ch4o5/sm3-php/src/functions.php',
    );

    public static $prefixLengthsPsr4 = array (
        'S' => 
        array (
            'SM3\\' => 4,
        ),
    );

    public static $prefixDirsPsr4 = array (
        'SM3\\' => 
        array (
            0 => __DIR__ . '/..' . '/ch4o5/sm3-php/src',
        ),
    );

    public static $classMap = array (
        'Composer\\InstalledVersions' => __DIR__ . '/..' . '/composer/InstalledVersions.php',
    );

    public static function getInitializer(ClassLoader $loader)
    {
        return \Closure::bind(function () use ($loader) {
            $loader->prefixLengthsPsr4 = ComposerStaticInit2619668dffef626b1919ad6e94bcd33e::$prefixLengthsPsr4;
            $loader->prefixDirsPsr4 = ComposerStaticInit2619668dffef626b1919ad6e94bcd33e::$prefixDirsPsr4;
            $loader->classMap = ComposerStaticInit2619668dffef626b1919ad6e94bcd33e::$classMap;

        }, null, ClassLoader::class);
    }
}
