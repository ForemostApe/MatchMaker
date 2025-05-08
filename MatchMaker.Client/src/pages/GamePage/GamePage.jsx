import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

export default function GamePage() {
  const { id } = useParams();
  const [game, setGame] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchGame = async () => {
      try {
        const response = await fetch(`/api/Game/${id}`);
        if (!response.ok) {
          throw new Error("Failed to fetch game");
        }
        const data = await response.json();
        setGame(data);
      } catch (err) {
        setError(err.message || "Something went wrong");
      } finally {
        setLoading(false);
      }
    };

    fetchGame();
  }, [id]);

  if (loading) {
    return <div className="p-4">Laddar spelinformation...</div>;
  }

  if (error) {
    return <div className="p-4 text-red-500">Fel: {error}</div>;
  }

  if (!game) {
    return <div className="p-4">Spelet kunde inte hittas.</div>;
  }

  return (
    <div className="container mx-auto px-4 py-6">
      <h1 className="text-2xl font-bold mb-4">Matchdetaljer</h1>

      <div className="bg-white rounded shadow p-6 space-y-4">
        <div>
          <h2 className="text-xl font-semibold">
            {game.homeTeam} vs {game.awayTeam}
          </h2>
          <p className="text-gray-600">
            Datum: {game.startTime ? new Date(game.startTime).toLocaleDateString() : "Saknas"}<br />
            Tid: {game.startTime && game.endTime
            ? `${new Date(game.startTime).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })} - ${new Date(game.endTime).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}`
            : "Saknas"}<br />
            Plats: {game.location || "Saknas"}
          </p>
        </div>

        {game.functionaries && game.functionaries.length > 0 && (
          <div>
            <h3 className="text-lg font-semibold mt-4">Funktionärer</h3>
            <ul className="list-disc pl-5">
              {game.functionaries.map((f) => (
                <li key={f.id}>
                  {f.name} ({f.role})
                </li>
              ))}
            </ul>
          </div>
        )}
      </div>
    </div>
  );
}
