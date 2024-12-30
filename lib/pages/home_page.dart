import 'package:flutter/material.dart';
import 'package:audioplayers/audioplayers.dart';
import 'achievements_page.dart';
import 'ar_page.dart';
import 'leaderboard_page.dart';

class HomePage extends StatefulWidget {
  const HomePage({super.key});

  @override
  State<StatefulWidget> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  int _currentIndex = 0;

  final List<Widget> _pages = [
    const _HomeContent(),
    const LeaderboardPage(),
    const AchievementsPage(),
  ];

  late AudioPlayer _audioPlayer;

  @override
  void initState() {
    super.initState();
    _audioPlayer = AudioPlayer();
    _playBackgroundMusic();
  }

  @override
  void dispose() {
    super.dispose();
    _audioPlayer.stop(); // Stop the music when the page is disposed
  }

  void _playBackgroundMusic() async {
    // Load and play the background music from a URL
    const String musicUrl =
        'https://marnickm.github.io/videoHostMM/medieval-ambient-236809.mp3';
    _audioPlayer.setReleaseMode(ReleaseMode.loop);
    await _audioPlayer.play(UrlSource(musicUrl));
  }

  void _stopBackgroundMusic() {
    _audioPlayer.stop();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: _pages[_currentIndex],
      bottomNavigationBar: BottomNavigationBar(
        currentIndex: _currentIndex,
        onTap: (index) {
          setState(() {
            _currentIndex = index;
          });
        },
        selectedItemColor:
            const Color(0xFFFF440E), // Set selected icon color to orange
        unselectedItemColor: Colors.grey, // Set unselected icon color
        items: const [
          BottomNavigationBarItem(
            icon: Icon(Icons.home),
            label: 'Home',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.leaderboard),
            label: 'Leaderboard',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.emoji_events),
            label: 'Achievements',
          ),
        ],
      ),
    );
  }
}

class _HomeContent extends StatefulWidget {
  const _HomeContent();

  @override
  _HomeContentState createState() => _HomeContentState();
}

class _HomeContentState extends State<_HomeContent>
    with TickerProviderStateMixin {
  late AnimationController _controller;
  late Animation<double> _animation;

  @override
  void initState() {
    super.initState();

    // Initialize the animation controller for the border effect
    _controller = AnimationController(
      duration: const Duration(seconds: 1),
      vsync: this,
    )..repeat(reverse: false); // Make it repeat continuously

    // Animation to loop from 0.0 to 1.0
    _animation = Tween<double>(begin: 0, end: 1).animate(
      CurvedAnimation(parent: _controller, curve: Curves.linear),
    );
  }

  @override
  void dispose() {
    _controller.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    // Access the parent widget's state
    final _HomePageState? homePageState =
        context.findAncestorStateOfType<_HomePageState>();

    return Center(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          GestureDetector(
            onTap: () {
              homePageState?._stopBackgroundMusic();
              Navigator.push(
                context,
                MaterialPageRoute(builder: (context) => const ArPage()),
              );
            },
            child: MouseRegion(
              cursor: SystemMouseCursors.click,
              child: AnimatedBuilder(
                animation: _animation,
                builder: (context, child) {
                  // Calculate the position of the "traveling" effect
                  double borderValue = _animation.value;

                  return AnimatedContainer(
                    duration: const Duration(milliseconds: 100),
                    curve: Curves.easeInOut,
                    decoration: BoxDecoration(
                      color: const Color(0xFFFF440E), // Button color
                      borderRadius:
                          BorderRadius.circular(30), // Rounded corners
                      border: Border.all(
                        width: 4,
                        color: Color.lerp(
                            Colors.orange, Colors.yellow, borderValue)!,
                      ), // Animate border color
                    ),
                    padding: const EdgeInsets.symmetric(
                        vertical: 20, horizontal: 60),
                    child: const Text(
                      'Play',
                      style: TextStyle(
                        fontSize: 32,
                        fontWeight: FontWeight.bold,
                        color: Colors.white,
                      ),
                    ),
                  );
                },
              ),
            ),
          ),
        ],
      ),
    );
  }
}
