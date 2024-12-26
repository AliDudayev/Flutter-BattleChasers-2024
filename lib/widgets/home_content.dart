import 'package:flutter/material.dart';

class HomeContent extends StatelessWidget {
  const HomeContent({super.key});

  @override
  Widget build(BuildContext context) {
    return Center(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Image.asset(
            'assets/images/logo.jpg',
            width: 150.0,
            height: 150.0,
          ),
          const SizedBox(height: 20.0),
          ElevatedButton(
            onPressed: () {
              // Navigate to Unity page placeholder
              ScaffoldMessenger.of(context).showSnackBar(
                const SnackBar(content: Text('Unity game launching soon!')),
              );
            },
            child: const Text('Play'),
          ),
        ],
      ),
    );
  }
}