import { useState } from "react";

const CreateGame = ({ onSubmit }) => {
  const [startTime, setStartTime] = useState("");
  const [endTime, setEndTime] = useState("");
  const [gameType, setGameType] = useState("");
  const [location, setLocation] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    const gameData = { startTime, endTime, gameType, location };
    
    try {
      await onSubmit(gameData);
      alert("Game created successfully!");
    } catch (error) {
      console.error("Error creating game:", error);
      alert("There was an error creating the game.");
    }
  };

  return (
    <form onSubmit={handleSubmit} className="max-w-md mx-auto p-6">
      <h2 className="text-2xl font-bold mb-4 text-gray-800">Planera match</h2>

      <div className="mb-4">
        <label htmlFor="startTime" className="block text-gray-700 mb-2">
          Starttid
        </label>
        <input
          type="datetime-local"
          id="startTime"
          value={startTime}
          onChange={(e) => setStartTime(e.target.value)}
          className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
          required
        />
      </div>

      <div className="mb-4">
        <label htmlFor="endTime" className="block text-gray-700 mb-2">
          Sluttid
        </label>
        <input
          type="datetime-local"
          id="endTime"
          value={endTime}
          onChange={(e) => setEndTime(e.target.value)}
          className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
          required
        />
      </div>

      <div className="mb-4">
        <label htmlFor="gameType" className="block text-gray-700 mb-2">
          Game Type????
        </label>
        <input
          type="text"
          id="gameType"
          value={gameType}
          onChange={(e) => setGameType(e.target.value)}
          className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
          required
        />
      </div>

      <div className="mb-4">
        <label htmlFor="location" className="block text-gray-700 mb-2">
          Plats
        </label>
        <input
          type="text"
          id="location"
          value={location}
          onChange={(e) => setLocation(e.target.value)}
          className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
          required
        />
      </div>

      <button
        type="submit"
        className="w-full bg-blue-600 text-white py-2 px-4 rounded-md hover:bg-blue-700 transition-colors"
      >
        Create Game
      </button>
    </form>
  );
};

export default CreateGame;
