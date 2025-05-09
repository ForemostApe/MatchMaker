import { useParams, useLocation } from "react-router-dom";
import useGameData from "../../hooks/useGameData";

const GameEdit = () => {
  const { id } = useParams();
  const location = useLocation();
  const { game, homeTeam, awayTeam, loading, error } = useGameData(id, location);

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error loading game data.</p>;

  return (
    <div className="p-4 max-w-3xl mx-auto bg-white shadow rounded">
      <h1 className="text-xl font-bold mb-4">
        Redigera {homeTeam.teamName} vs {awayTeam.teamName}
      </h1>
      <form>
        <label className="block mb-2">
          Plats:
          <input
            type="text"
            defaultValue={game.location}
            className="w-full border rounded p-2"
          />
        </label>
        <button type="submit" className="mt-4 px-4 py-2 bg-blue-600 text-white rounded">
          Spara ändringar
        </button>
      </form>
    </div>
  );
};

export default GameEdit;
