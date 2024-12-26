import 'package:sqflite/sqflite.dart';
import 'package:path/path.dart';

class DatabaseHelper {
  static Future<Database> _openDatabase() async {
    return openDatabase(
      join(await getDatabasesPath(), 'battle_chasers.db'),
      onCreate: (db, version) {
        db.execute(
            'CREATE TABLE users(id INTEGER PRIMARY KEY, username TEXT, score INTEGER, killcount INTEGER, dragonsKilled TEXT)');
        db.execute('CREATE TABLE dragons(id TEXT PRIMARY KEY, name TEXT)');
      },
      version: 1,
    );
  }

  static Future<void> insertUser(String username, int score, int killCount,
      List<String> dragonsKilled) async {
    final db = await _openDatabase();
    await db.insert(
      'users',
      {
        'username': username,
        'score': score,
        'killcount': killCount,
        'dragonsKilled':
            dragonsKilled.join(','), // Convert list to a comma-separated string
      },
      conflictAlgorithm: ConflictAlgorithm
          .replace, // Replace existing record if username already exists
    );
    print('User $username inserted');
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

  static Future<void> insertDragon(String id, String name) async {
    final db = await _openDatabase();
    await db.insert(
      'dragons',
      {'id': id, 'name': name},
      conflictAlgorithm: ConflictAlgorithm.ignore,
    );
  }

  static Future<List<Map<String, dynamic>>> fetchAllDragons() async {
    final db = await _openDatabase();
    return db.query('dragons');
  }
}
