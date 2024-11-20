import 'package:flutter/material.dart';
import 'pages/home_page.dart';
import 'pages/achievements_page.dart';

void main() {
  runApp(const BattleChasersApp());
}

class BattleChasersApp extends StatelessWidget {
  const BattleChasersApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      title: 'Battle Chasers',
      theme: ThemeData(
        // Global background color for the app
        scaffoldBackgroundColor: const Color(0xFF1C2834), // #1c2834
        appBarTheme: const AppBarTheme(
          backgroundColor: Color(0xFF1C2834), // App bar color #1c2834
        ),
        // Primary color for buttons and borders
        primaryColor: const Color(0xFFFF440E), // #ff440e
        visualDensity: VisualDensity.adaptivePlatformDensity,
        // Button Theme (global for all buttons)
        elevatedButtonTheme: ElevatedButtonThemeData(
          style: ElevatedButton.styleFrom(
            backgroundColor: const Color(0xFFFF440E), // Button color #ff440e
            foregroundColor: Colors.white, // Button text color white
          ),
        ),
        // Input decoration theme for all text fields
        inputDecorationTheme: const InputDecorationTheme(
          labelStyle:  TextStyle(color: Colors.white), // White labels
          enabledBorder:  OutlineInputBorder(
            borderSide: BorderSide(color: Color(0xFFFF440E)), // Border color #ff440e
          ),
          focusedBorder:  OutlineInputBorder(
            borderSide: BorderSide(color: Color(0xFFFF440E)), // Border color #ff440e
          ),
        ),
        // Text theme for the whole app
        textTheme: const TextTheme(
          headlineSmall:  TextStyle(color: Colors.white),
          titleMedium:  TextStyle(color: Colors.white),
          bodyLarge:  TextStyle(color: Colors.white), // White body text
        ),
      ),
      initialRoute: '/',
      routes: {
        '/': (context) => const HomePage(),
        '/achievements': (context) => const AchievementsPage(),
      },
    );
  }
}
