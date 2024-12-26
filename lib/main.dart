import 'package:flutter/material.dart';
import 'pages/home_page.dart';
import './database_helper.dart';

void main() {
  runApp(const BattleChasersApp());
  // clearData();
  // insertInitialData();
  initializeDragons();
}

  // clear the database and insert initial data
  Future<void> clearData() async {
    // await DatabaseHelper.fetchUserData();
  }

  Future<void> insertInitialData() async {
    await DatabaseHelper.insertUser('Marnick', 500, 5, ['TerrorBringer Albino']);
    await DatabaseHelper.insertUser('Ali', 250, 5, ['Usurper Blue']);
}
  Future<void> initializeDragons() async {
    final dragons = [
      {'id': 'TerrorBringer_Albino', 'name': 'TerrorBringer Albino'},
      {'id': 'TerrorBringer_Grey', 'name': 'TerrorBringer Grey'},
      {'id': 'TerrorBringer_Green', 'name': 'TerrorBringer Green'},
      {'id': 'TerrorBringer_Red', 'name': 'TerrorBringer Red'},
      {'id': 'Usurper_Albino', 'name': 'Usurper Albino'},
      {'id': 'Usurper_Blue', 'name': 'Usurper Blue'},
      {'id': 'Usurper_Dark', 'name': 'Usurper Dark'},
      {'id': 'Usurper_Red', 'name': 'Usurper Red'},
      {'id': 'SoulEater_Blue', 'name': 'SoulEater Blue'},
      {'id': 'SoulEater_Green', 'name': 'SoulEater Green'},
      {'id': 'SoulEater_Purple', 'name': 'SoulEater Purple'},
      {'id': 'SoulEater_Red', 'name': 'SoulEater Red'},
      {'id': 'Nightmare_Albino', 'name': 'Nightmare Albino'},
      {'id': 'Nightmare_Blue', 'name': 'Nightmare Blue'},
      {'id': 'Nightmare_Green', 'name': 'Nightmare Green'},
      {'id': 'Nightmare_Red', 'name': 'Nightmare Red'},
    ];
  
    for (var dragon in dragons) {
      await DatabaseHelper.insertDragon(dragon['id']!, dragon['name']!);
    }
}


class BattleChasersApp extends StatelessWidget {
  const BattleChasersApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      title: 'Battle Chasers',
      theme: ThemeData(
        scaffoldBackgroundColor: const Color(0xFF1C2834),
        appBarTheme: const AppBarTheme(backgroundColor: Color(0xFF1C2834)),
        primaryColor: const Color(0xFFFF440E),
        visualDensity: VisualDensity.adaptivePlatformDensity,
        elevatedButtonTheme: ElevatedButtonThemeData(
          style: ElevatedButton.styleFrom(
            backgroundColor: const Color(0xFFFF440E),
            foregroundColor: Colors.white,
          ),
        ),
        textTheme: const TextTheme(
          headlineSmall: TextStyle(color: Colors.white),
          titleMedium: TextStyle(color: Colors.white),
          bodyLarge: TextStyle(color: Colors.white),
        ),
      ),
      home: const HomePage(),
    );
  }
}
