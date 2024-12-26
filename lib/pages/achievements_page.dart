import 'package:flutter/material.dart';
import '../database_helper.dart';

class AchievementsPage extends StatefulWidget {
  const AchievementsPage({super.key});

  @override
  // ignore: library_private_types_in_public_api
  _AchievementsPageState createState() => _AchievementsPageState();
}

class _AchievementsPageState extends State<AchievementsPage> {
  String? selectedUsername;
  Map<String, dynamic>? userData;
  List<Map<String, dynamic>> allDragons = [];
  bool isLoading = true;

  @override
  void initState() {
    super.initState();
    _fetchInitialData();
  }

  Future<void> _fetchInitialData() async {
    allDragons = await DatabaseHelper.fetchAllDragons();
    setState(() {
      isLoading = false;
    });
  }

  Future<void> _fetchUserData(String username) async {
    final data = await DatabaseHelper.fetchUserData(username);
    setState(() {
      userData = data;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Achievements'),
      ),
      body: isLoading
          ? const Center(child: CircularProgressIndicator())
          : Padding(
              padding: const EdgeInsets.all(16.0),
              child: SingleChildScrollView(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    const Text(
                      'Select User',
                      style: TextStyle(
                        fontSize: 18,
                        fontWeight: FontWeight.bold,
                        color: Colors.white,
                      ),
                    ),
                    const SizedBox(height: 10),
                    FutureBuilder<List<Map<String, dynamic>>>(
                      future: DatabaseHelper.fetchAllUsers(),
                      builder: (context, snapshot) {
                        if (snapshot.connectionState ==
                            ConnectionState.waiting) {
                          return const Center(
                            child: CircularProgressIndicator(),
                          );
                        } else if (snapshot.hasError ||
                            snapshot.data == null ||
                            snapshot.data!.isEmpty) {
                          return const Text(
                            'No users available.',
                            style: TextStyle(color: Colors.white),
                          );
                        } else {
                          final users = snapshot.data!;
                          return DropdownButton<String>(
                            value: selectedUsername,
                            isExpanded: true,
                            dropdownColor: const Color(0xFF1C2834),
                            style: const TextStyle(color: Colors.white),
                            items: users
                                .map(
                                  (user) => DropdownMenuItem<String>(
                                    value: user['username'],
                                    child: Text(user['username']),
                                  ),
                                )
                                .toList(),
                            onChanged: (value) {
                              setState(() {
                                selectedUsername = value;
                                userData = null;
                              });
                              if (value != null) {
                                _fetchUserData(value);
                              }
                            },
                          );
                        }
                      },
                    ),
                    const SizedBox(height: 20),
                    if (userData != null) ...[
                      Text(
                        '${userData!['username']}\'s Achievements',
                        style: Theme.of(context).textTheme.headlineSmall,
                      ),
                      const SizedBox(height: 10),
                      Text(
                        'Best Score: ${userData!['score']}',
                        style: Theme.of(context).textTheme.bodyLarge,
                      ),
                      Text(
                        'Total Enemies Killed: ${userData!['killcount']}',
                        style: Theme.of(context).textTheme.bodyLarge,
                      ),
                      const SizedBox(height: 20),
                      const Text(
                        'Defeated Dragons',
                        style: TextStyle(
                          fontSize: 16,
                          fontWeight: FontWeight.bold,
                          color: Colors.white,
                        ),
                      ),
                      const SizedBox(height: 10),
                      ...allDragons.map((dragon) {
                        final dragonName = dragon['name'];
                        final isDefeated =
                            (userData!['dragonsKilled'] as String)
                                .split(',')
                                .contains(dragon['id']);
                        return _buildAchievementCard(dragonName, isDefeated);
                      }),
                    ],
                  ],
                ),
              ),
            ),
    );
  }

  Widget _buildAchievementCard(String dragonName, bool isAchieved) {
    return Card(
      color: const Color(0xFF2A3A4D),
      child: ListTile(
        title: Text(
          dragonName,
          style: const TextStyle(color: Colors.white),
        ),
        trailing: Icon(
          isAchieved ? Icons.check : Icons.close,
          color: isAchieved ? Colors.green : Colors.red,
        ),
      ),
    );
  }
}
