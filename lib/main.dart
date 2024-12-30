import 'package:flutter/material.dart';
import 'pages/home_page.dart';
import './database_helper.dart';

void main() {
  runApp(const BattleChasersApp());
  //clearData();
  insertInitialData();
  //DatabaseHelper.resetDatabase();
  initializeDragons();
}

// clear the database and insert initial data
Future<void> clearData() async {
  await DatabaseHelper.clearAllUsers();
}

Future<void> insertInitialData() async {
  await DatabaseHelper.insertUser(
      'Marnick', 100, 50, ['Red_Usurper', 'Blue_Usurper']);
  await DatabaseHelper.insertUser('Ali', 150, 25, ['Gold_Usurper']);
  // await DatabaseHelper.insertUser('Player2', 200, 75, ['Nightmare_Usurper']);
}

Future<void> initializeDragons() async {
  // List of dragon names, IDs, and their respective video URLs
  final dragons = [
    {
      'id': 'Violet_Usurper',
      'name': 'Violet Usurper',
      'videoUrl': 'https://marnickm.github.io/videoHostMM/Violet_Usurper.mp4'
    },
    {
      'id': 'Red_Usurper',
      'name': 'Red Usurper',
      'videoUrl': 'https://marnickm.github.io/videoHostMM/Red_Usurper.mp4'
    },
    {
      'id': 'Poison_Usurper',
      'name': 'Poison Usurper',
      'videoUrl': 'https://marnickm.github.io/videoHostMM/Poison_Usurper.mp4'
    },
    {
      'id': 'Nightmare_Usurper',
      'name': 'Nightmare Usurper',
      'videoUrl': 'https://marnickm.github.io/videoHostMM/Nightmare_Usurper.mp4'
    },
    {
      'id': 'Lava_Usurper',
      'name': 'Lava Usurper',
      'videoUrl': 'https://marnickm.github.io/videoHostMM/Lava_Usurper.mp4'
    },
    {
      'id': 'Ice_Usurper',
      'name': 'Ice Usurper',
      'videoUrl': 'https://marnickm.github.io/videoHostMM/Ice_Usurper.mp4'
    },
    {
      'id': 'Gold_Usurper',
      'name': 'Gold Usurper',
      'videoUrl': 'https://marnickm.github.io/videoHostMM/Gold_Usurper.mp4'
    },
    {
      'id': 'Dark_Usurper',
      'name': 'Dark Usurper',
      'videoUrl': 'https://marnickm.github.io/videoHostMM/Dark_Usurper.mp4'
    },
    {
      'id': 'Blue_Usurper',
      'name': 'Blue Usurper',
      'videoUrl': 'https://marnickm.github.io/videoHostMM/Blue_Usurper.mp4'
    },
    {
      'id': 'Azure_Usurper',
      'name': 'Azure Usurper',
      'videoUrl': 'https://marnickm.github.io/videoHostMM/Azure_Usurper.mp4'
    },
    {
      'id': 'Amethyst_Usurper',
      'name': 'Amethyst Usurper',
      'videoUrl': 'https://marnickm.github.io/videoHostMM/Amethyst_Usurper.mp4'
    },
    {
      'id': 'Albino_Usurper',
      'name': 'Albino Usurper',
      'videoUrl': 'https://marnickm.github.io/videoHostMM/Albino_Usurper.mp4'
    },
  ];

  // Insert dragons into the database with their video URLs
  for (var dragon in dragons) {
    final videoUrl = dragon['videoUrl'];

    // Cast 'id' and 'name' to String
    await DatabaseHelper.insertDragon(
        dragon['id'] as String, dragon['name'] as String, videoUrl);
  }
}

// class BattleChasersApp extends StatelessWidget {
//   const BattleChasersApp({super.key});

//   @override
//   Widget build(BuildContext context) {
//     return MaterialApp(
//       debugShowCheckedModeBanner: false,
//       title: 'Battle Chasers',
//       theme: ThemeData(
//         scaffoldBackgroundColor: const Color(0xFF1C2834),
//         // No AppBar theme defined, ensuring no AppBar will appear
//         primaryColor: const Color(0xFFFF440E),
//         visualDensity: VisualDensity.adaptivePlatformDensity,
//         elevatedButtonTheme: ElevatedButtonThemeData(
//           style: ElevatedButton.styleFrom(
//             backgroundColor: const Color(0xFFFF440E),
//             foregroundColor: Colors.white,
//           ),
//         ),
//         textTheme: const TextTheme(
//           headlineSmall: TextStyle(color: Colors.white),
//           titleMedium: TextStyle(color: Colors.white),
//           bodyLarge: TextStyle(color: Colors.white),
//         ),
//       ),
//       home: const HomePage(), // Your home page widget
//     );
//   }
// }

class BattleChasersApp extends StatelessWidget {
  const BattleChasersApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      theme: ThemeData(
        scaffoldBackgroundColor: const Color(0xFF1C2834),
        // No AppBar theme defined, ensuring no AppBar will appear
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
      home: const HomePage(), // Your home page widget
    );
  }
}
