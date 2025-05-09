import { useParams, useLocation } from "react-router-dom";
import useGameData from "../../hooks/useGameData";

const GamePage = () => {
  const { id } = useParams();
  const location = useLocation();
  const { game, homeTeam, awayTeam, loading, error } = useGameData(id, location);

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error loading game data.</p>;

  return (
    <div className="p-4 max-w-3xl mx-auto bg-white shadow rounded">
      <h1 className="text-xl font-bold mb-4">
        {homeTeam.teamName} vs {awayTeam.teamName}
      </h1>
      <p><strong>Start:</strong> {new Date(game.startTime).toLocaleString()}</p>
      <p><strong>Slut:</strong> {new Date(game.endTime).toLocaleString()}</p>
      <p><strong>Plats:</strong> {game.location}</p>
      <p><strong>Status:</strong> {game.gameStatus}</p>
    </div>
  );
};

export default GamePage;
