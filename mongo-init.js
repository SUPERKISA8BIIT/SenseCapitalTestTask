db = new Mongo().getDB("tick_crossdb");

db.createCollection('GamesCollection', { capped: false });