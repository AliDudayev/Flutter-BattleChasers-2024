import 'dart:convert';
import 'package:battle_chasers/pages/achievements_page.dart';
import 'package:battle_chasers/pages/home_page.dart';
import 'package:flutter/material.dart';
import 'package:flutter_unity_widget/flutter_unity_widget.dart';
import 'package:permission_handler/permission_handler.dart';
import '../database_helper.dart';

class ArPage extends StatefulWidget {
  const ArPage({super.key});

  @override
  State<ArPage> createState() => _ArPageState();
}

class _ArPageState extends State<ArPage> {
  UnityWidgetController? _unityWidgetController;
  bool _isCameraPermissionGranted = false;

  @override
  void initState() {
    super.initState();
    _checkCameraPermission();
  }

  Future<void> _checkCameraPermission() async {
    PermissionStatus status = await Permission.camera.status;
    if (!status.isGranted) {
      status = await Permission.camera.request();
    }

    if (status.isGranted) {
      setState(() {
        _isCameraPermissionGranted = true;
      });
    }
  }

  @override
  void dispose() {
    print("Debug: Disposing Unity widget"); // Debug: Before disposing Unity
    _unityWidgetController?.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    if (!_isCameraPermissionGranted) {
      return Scaffold(
        appBar: AppBar(
          title: const Text('Enable camera'),
        ),
        body: const Center(
          child: Text('Camera permission is required to proceed.'),
        ),
      );
    }

    return Scaffold(
      body: UnityWidget(
        onUnityCreated: _onUnityCreated,
        onUnityMessage: onUnityMessage,
        useAndroidViewSurface: true,
      ),
    );
  }

  void _onUnityCreated(UnityWidgetController controller) {
    _unityWidgetController = controller;
    print('Unity widget initialized.');
  }

  void onUnityMessage(message) {
    print("Debug: Received message from Unity - $message");

    print("Debug: Unity requested to close.");

    // Unload the Unity view and dispose of the controller
    _unityWidgetController?.unload();
    _unityWidgetController?.dispose();

    // Navigate to the desired Flutter screen (e.g., HomePage)
    Navigator.of(context).pushAndRemoveUntil(
      MaterialPageRoute(builder: (context) => const HomePage()),
      (Route<dynamic> route) => false,
    );

    print("Debug: Received message from Unity - $message");

    if (message == null || message.isEmpty) {
      print("Debug: Message is empty or null.");
    }

    // Attempt to decode the message
    try {
      Map<String, dynamic> decodedMessage = json.decode(message);
      print("Debug: Decoded message - $decodedMessage");

      if (decodedMessage.containsKey('score') &&
          decodedMessage.containsKey('count')) {
        int score = decodedMessage['score'];
        List<String> killedDragons =
            List<String>.from(decodedMessage['killedDragons']);
        int killCount = int.parse(decodedMessage['count']);

        _handleUnityResults(score, killedDragons, killCount);
      } else {
        print("Debug: Received message does not contain expected keys.");
      }
    } catch (e) {
      print("Debug: Error decoding message: $e");
    }
  }

  void _handleUnityResults(
      int score, List<String> killedDragons, int killCount) {
    // Debug: Checking the received data
    print("Debug: Handling Unity results");
    print(
        "Score: $score, Killed Dragons: $killedDragons, Kill Count: $killCount");

    // Show dialog to ask for the player's name and display the results
    _promptForName(score, killedDragons, killCount);
  }

  // void onUnityMessage(message) {
  //   print(
  //       "Debug: Received message from Unity - $message"); // Debug: Message received

  //   try {
  //     Map<String, dynamic> decodedMessage = json.decode(message);
  //     print("Debug: Decoded message - $decodedMessage"); // Debug: JSON decoded

  //     if (decodedMessage.containsKey('score') &&
  //         decodedMessage.containsKey('killedDragons') &&
  //         decodedMessage.containsKey('count')) {
  //       int score = decodedMessage['score'];
  //       List<String> killedDragons =
  //           List<String>.from(decodedMessage['killedDragons']);
  //       int killCount = int.parse(decodedMessage['count']);

  //       print(
  //           "Debug: Score: $score, Killed Dragons: $killedDragons, Kill Count: $killCount"); // Debug: Extracted data

  //       // Handle results and clean up Unity
  //       // _handleUnityResults(score, killedDragons, killCount);
  //     }
  //   } catch (e) {
  //     print('Error parsing message from Unity: $e');
  //   }
  //   dispose();
  // }

  // void _handleUnityResults(
  //     int score, List<String> killedDragons, int killCount) {
  //   // Save results or show a dialog if needed
  //   _promptForName(score, killedDragons, killCount);

  //   // Close Unity widget
  //   _unityWidgetController?.unload();
  //   _unityWidgetController?.dispose();

  //   // Navigate to the desired Flutter screen
  //   Navigator.of(context).pushAndRemoveUntil(
  //     MaterialPageRoute(builder: (context) => const HomePage()),
  //     (Route<dynamic> route) => false,
  //   );
  // }

  // void onUnityMessage(message) {
  //   try {
  //     Map<String, dynamic> decodedMessage = json.decode(message);

  //     if (decodedMessage.containsKey('score') &&
  //         decodedMessage.containsKey('killedDragons') &&
  //         decodedMessage.containsKey('count')) {
  //       int score = decodedMessage['score'];
  //       List<String> killedDragons =
  //           List<String>.from(decodedMessage['killedDragons']);
  //       int killCount = int.parse(decodedMessage['count']);

  //       _promptForName(score, killedDragons, killCount);
  //     }
  //   } catch (e) {
  //     print('Error parsing message from Unity: $e');
  //   }
  // }

  void _promptForName(int score, List<String> killedDragons, int killCount) {
    TextEditingController nameController = TextEditingController();

    Future<void> _saveGameResult(String name, int score, int killCount,
        List<String> killedDragons) async {
      await DatabaseHelper.insertUser(name, score, killCount, killedDragons);
      print(
          'Game result saved: $name, Score: $score, Kill Count: $killCount, Killed Dragons: $killedDragons');
    }

    showDialog(
      context: context,
      barrierDismissible: false,
      builder: (BuildContext context) {
        return AlertDialog(
          contentPadding:
              const EdgeInsets.symmetric(vertical: 10, horizontal: 20),
          content: SizedBox(
            width: 250, // Control the width of the dialog content
            child: TextField(
              controller: nameController,
              // make the input text color the same as the rest of this dialog
              style: TextStyle(
                  color: Theme.of(context).textTheme.bodyLarge!.color),
            ),
          ),
          actions: [
            const Text("Enter name above |"),
            // Submit Button
            TextButton(
              onPressed: () {
                String name = nameController.text.trim();
                if (name.isNotEmpty) {
                  _saveGameResult(name, score, killCount, killedDragons);
                  Navigator.of(context).pop();
                } else {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(content: Text("Name cannot be empty!")),
                  );
                }
              },
              child:
                  const Text("Submit", style: TextStyle(color: Colors.black)),
            ),
            // Navigate to HomePage Button
            TextButton(
              onPressed: () {
                Navigator.of(context).pushAndRemoveUntil(
                  MaterialPageRoute(builder: (context) => const HomePage()),
                  (Route<dynamic> route) => false,
                );
              },
              child: const Text("Home", style: TextStyle(color: Colors.black)),
            ),
          ],
        );
      },
    );
  }
}
