export default function GameView({ game }) {
    return (
      <div className="space-y-2">
        <h1 className="text-xl font-bold">
          {game.homeTeam} vs {game.awayTeam}
        </h1>
        <p>{game.date} at {game.time}</p>
        <p className="text-gray-600">{game.place}</p>
      </div>
    );
  }
  