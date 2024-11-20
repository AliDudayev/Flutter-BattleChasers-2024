import 'package:flutter/material.dart';
import '../global.dart';

class AchievementsPage extends StatelessWidget {
  const AchievementsPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Achievements'),
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              'Hello, ${username ?? "Player"}!',
              style: Theme.of(context).textTheme.headlineSmall,
            ),
            const SizedBox(height: 20.0),
            Text(
              'Your Achievements:',
              style: Theme.of(context).textTheme.titleMedium,
            ),
            const SizedBox(height: 10.0),
            const Card(
              child: ListTile(
                title: Text('Total Battles Won'),
                trailing: Text('0'),
              ),
            ),
            const Card(
              child: ListTile(
                title: Text('Enemies Defeated'),
                trailing: Text('0'),
              ),
            ),
            const Card(
              child: ListTile(
                title: Text('Rank'),
                trailing: Text('Unranked'),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
