import 'dart:io';
import 'package:sqflite/sqflite.dart';
import 'package:path/path.dart';

class DatabaseHelper {
  static Future<Database> _openDatabase() async {
    return openDatabase(
      join(await getDatabasesPath(), 'battle_chasers.db'),
      onCreate: (db, version) {
        db.execute(
            'CREATE TABLE users(id INTEGER PRIMARY KEY, username TEXT, score INTEGER, killcount INTEGER, dragonsKilled TEXT, highscore INTEGER)');
        db.execute(
            'CREATE TABLE dragons(id TEXT PRIMARY KEY, name TEXT, videoUrl TEXT)');
      },
      version: 1,
    );
  }

  static Future<void> insertUser(String username, int score, int killCount,
      List<String> dragonsKilled) async {
    final db = await _openDatabase();
    // Fetch existing user data with a case-insensitive username match
    final existingUser = await db.query(
      'users',
      where: 'username = ?', // Case-insensitive match
      whereArgs: [username],
    );

    if (existingUser.isNotEmpty) {
      // User already exists, update their data
      final user = existingUser.first;

      // Parse current data
      int currentScore = user['score'] as int;
      int currentKillCount = user['killcount'] as int;
      int currentHighscore = user['highscore'] as int;

      List<String> currentDragonsKilled =
          (user['dragonsKilled'] as String).split(',');

      // Update values
      int newScore = currentScore + score;
      int newKillCount = currentKillCount + killCount;

      // Merge dragon lists, avoiding duplicates
      List<String> updatedDragonsKilled = {
        ...currentDragonsKilled,
        ...dragonsKilled
            .map((dragon) => dragon.replaceAll(RegExp(r'\(Clone\)'), '').trim())
      }.toList();

      int highscore = (score > currentHighscore) ? score : currentHighscore;

      // Update the database
      await db.update(
        'users',
        {
          'username': username,
          'score': newScore,
          'killcount': newKillCount,
          'dragonsKilled': updatedDragonsKilled.join(','),
          'highscore': highscore,
        },
        where: 'username = ?',
        whereArgs: [username],
      );

      print(
          'User $username updated with new score $newScore, kill count $newKillCount, dragons killed $updatedDragonsKilled, highscore $highscore');
    } else {
      // User does not exist, insert a new record
      List<String> cleanedDragons = dragonsKilled.map((dragon) {
        return dragon.replaceAll(RegExp(r'\(Clone\)'), '').trim();
      }).toList();

      await db.insert(
        'users',
        {
          'username': username,
          'score': score,
          'killcount': killCount,
          'dragonsKilled': cleanedDragons.join(','),
          'highscore': score,
        },
        conflictAlgorithm: ConflictAlgorithm.replace,
      );

      print(
          'User $username inserted with score $score, kill count $killCount, dragons killed $dragonsKilled, highscore $score');
    }
  }

  static Future<List<Map<String, dynamic>>> fetchAllUsers() async {
    final db = await _openDatabase();
    return db.query('users', orderBy: 'score DESC');
  }

  static Future<Map<String, dynamic>?> fetchUserData(String username) async {
    final db = await _openDatabase();
    final result = await db.query(
      'users',
      where: 'username = ?',
      whereArgs: [username],
      limit: 1,
    );
    if (result.isNotEmpty) {
      return result.first;
    }
    return null;
  }

  static Future<void> insertDragon(
      String id, String name, String? videoUrl) async {
    final db = await _openDatabase();
    await db.insert(
      'dragons',
      {'id': id, 'name': name, 'videoUrl': videoUrl},
      conflictAlgorithm: ConflictAlgorithm.ignore,
    );
    print('Dragon $name inserted with ID $id and video URL $videoUrl');
  }

  static Future<List<Map<String, dynamic>>> fetchAllDragons() async {
    final db = await _openDatabase();
    return db.query('dragons');
  }

  static Future<void> resetDatabase() async {
    // Get the path to the database
    final dbPath = join(await getDatabasesPath(), 'battle_chasers.db');

    // Check if the database exists
    final dbFile = File(dbPath);
    if (await dbFile.exists()) {
      // Delete the database file
      await dbFile.delete();
      print('Database deleted successfully.');
    } else {
      print('No existing database found to delete.');
    }

    // Rebuild the database by reopening it
    await _openDatabase();
    print('Database recreated successfully.');
  }

  static Future<void> clearAllUsers() async {
    final db = await _openDatabase();

    // Delete all rows from the 'users' table
    await db.delete('users');

    print('All users have been cleared from the database.');
  }
}
