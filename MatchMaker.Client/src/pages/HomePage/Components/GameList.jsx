import { format, parseISO } from "date-fns";
import { sv } from "date-fns/locale";

const GameList = ({ games, teams, onGameClick, currentMonth }) => {
  const formattedMonth =
    format(currentMonth, "MMMM yyyy", { locale: sv }).charAt(0).toUpperCase() +
    format(currentMonth, "MMMM yyyy", { locale: sv }).slice(1);

  const bookedGames = games.filter(game => game.gameStatus === "Booked");

  return (
    <div className="w-full max-w-4xl bg-gray-50 rounded-lg shadow-md p-4 sm:p-6">
      <h3 className="text-md sm:text-xl font-bold mb-4">Matcher i {formattedMonth}</h3>
      {bookedGames.length > 0 ? (
        <ul className="space-y-4">
          {bookedGames.map((game) => {
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
        <p className="text-gray-600 text-sm sm:text-base">Inga matcher bokade.</p>
      )}
    </div>
  );
};

export default GameList;
