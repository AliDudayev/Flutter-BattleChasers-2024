import 'package:flutter/material.dart';
import '../global.dart';

class HomePage extends StatefulWidget {
  const HomePage({super.key});

  @override
  State<StatefulWidget> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  final _formKey = GlobalKey<FormState>();
  final _nameController = TextEditingController();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Battle Chasers'),
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Display the logo at the top
            Center(
              child: Image.asset(
                'assets/images/logo.jpg', // Path to the logo in your assets
                width: 150.0,  // Adjust the size as needed
                height: 150.0, // Adjust the size as needed
              ),
            ),
            const SizedBox(height: 16.0),
            const Text(
              'Welcome to Battle Chasers!',
              style:  TextStyle(
                fontFamily: 'KnightWarrior', // Custom font
                fontSize: 24,                // Font size
                color: Colors.white,         // Font color
              ),
            ),
            const SizedBox(height: 16.0),
            Form(
              key: _formKey,
              child: TextFormField(
                controller: _nameController,
                decoration: const InputDecoration(
                  labelText: 'Enter your name',
                  border: OutlineInputBorder(),
                ),
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Please enter a name.';
                  }
                  return null;
                },
                style: const TextStyle(
                  fontFamily: 'KnightWarrior', // Custom font in the text input
                  fontSize: 18,                // Adjust the input text size if needed
                ),
              ),
            ),
            const SizedBox(height: 20.0),
            ElevatedButton(
              onPressed: () {
                if (_formKey.currentState!.validate()) {
                  setState(() {
                    username = _nameController.text;
                  });
                  Navigator.pushNamed(context, '/achievements');
                }
              },
              child: const Text('Submit'),
            ),
          ],
        ),
      ),
    );
  }
}
