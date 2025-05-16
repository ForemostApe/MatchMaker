import { format, parseISO } from "date-fns";

const PendingGamesList = ({ games, teams, user, onGameClick }) => {
  if (!user) return null;

  const pendingGames = games.filter((game) => {
    const isPlanned = game.gameStatus === "Planned";
    const isSigned = game.gameStatus === "Signed";

console.log({
  gameId: game.id,
  status: game.gameStatus,
  referee: game.refereeId,
  userId: user.id,
  userRole: user.userRole
});

    if (user.userRole === "Coach") {
      return (
        isPlanned &&
        game.awayTeamId === user.teamAffiliation
      );
    }

    if (user.userRole === "Referee") {
      return (
        isSigned &&
        game.refereeId === user.id
      );
    }

    return false;
  });

  return (
    <div className="w-full max-w-4xl bg-gray-50 rounded-lg shadow-md p-4 sm:p-6 mb-5">
      <h3 className="text-md sm:text-xl font-bold mb-4">
        Matcher att hantera
      </h3>
      {pendingGames.length > 0 ? (
        <ul className="space-y-4">
          {pendingGames.map((game) => {
            const homeTeam = teams[game.homeTeamId] || { teamName: "Okänt hemmalag" };
            const awayTeam = teams[game.awayTeamId] || { teamName: "Okänt bortalag" };

            return (
              <li
                key={game.id}
                onClick={() => onGameClick(game)}
                className="border rounded p-3 bg-white shadow-sm text-sm md:text-base cursor-pointer hover:bg-gray-100 transition"
              >
                <p className="font-semibold">
                  {homeTeam.teamName} vs {awayTeam.teamName}
                </p>
                <p>
                  {format(parseISO(game.startTime), "yyyy-MM-dd")} Kl.{" "}
                  {format(parseISO(game.startTime), "HH:mm")}
                </p>
                <p className="text-gray-600">{game.place}</p>
              </li>
            );
          })}
        </ul>
      ) : (
        <p className="text-gray-600 text-sm sm:text-base">
          Inga matcher att hantera.
        </p>
      )}
    </div>
  );
};

export default PendingGamesList;
