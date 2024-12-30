import 'package:flutter/material.dart';
import 'package:video_player/video_player.dart';
import 'package:flutter/services.dart';

class VideoPage extends StatefulWidget {
  final String dragonId;
  final String videoUrl;
  final String dragonName;

  const VideoPage({
    super.key,
    required this.dragonId,
    required this.videoUrl,
    required this.dragonName,
  });

  @override
  _VideoPageState createState() => _VideoPageState();
}

class _VideoPageState extends State<VideoPage> {
  late VideoPlayerController _controller;

  @override
  void initState() {
    super.initState();
    _controller = VideoPlayerController.networkUrl(Uri.parse(widget.videoUrl))
      ..initialize().then((_) {
        setState(() {});
      });

    // Make the status bar and app bar transparent
    SystemChrome.setSystemUIOverlayStyle(
      const SystemUiOverlayStyle(
        statusBarColor: Colors.transparent, // Set status bar to transparent
        statusBarIconBrightness:
            Brightness.light, // Light icons on transparent background
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    // Get the full screen height and width
    final screenHeight = MediaQuery.of(context).size.height;
    final screenWidth = MediaQuery.of(context).size.width;

    return Scaffold(
      appBar: AppBar(
        backgroundColor: Colors.transparent, // Transparent app bar
        elevation: 0, // Remove app bar shadow
        title: Text(
          widget.dragonName,
          style:
              const TextStyle(color: Colors.white), // White text for visibility
        ),
        leading: IconButton(
          icon: const Icon(
            Icons.arrow_back,
            color: Colors.white, // Set the back arrow to white
          ),
          onPressed: () {
            Navigator.pop(context); // Go back to the previous screen
          },
        ),
      ),
      extendBodyBehindAppBar:
          true, // Allow the body to extend behind the app bar
      body: Stack(
        children: [
          // Background video that goes behind the app bar
          _controller.value.isInitialized
              ? Positioned(
                  top: 0,
                  left: 0,
                  child: SizedBox(
                    height: screenHeight, // Fill the screen height
                    width: screenWidth, // Fill the screen width
                    child: AspectRatio(
                      aspectRatio: _controller.value.aspectRatio,
                      child: VideoPlayer(_controller),
                    ),
                  ),
                )
              : const Center(child: CircularProgressIndicator()),

          // Foreground content (buttons, title, etc.)
          Positioned(
            bottom: 20, // Move the button to the bottom
            left: 20, // Align the button to the left side
            child: IconButton(
              icon: Icon(
                _controller.value.isPlaying ? Icons.pause : Icons.play_arrow,
                color: Colors.white,
                size: 50,
              ),
              onPressed: () {
                setState(() {
                  if (_controller.value.isPlaying) {
                    _controller.pause();
                  } else {
                    _controller.play();
                  }
                });
              },
            ),
          ),
        ],
      ),
    );
  }

  @override
  void dispose() {
    _controller.dispose();
    super.dispose();
  }
}
