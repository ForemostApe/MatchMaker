import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import {
  format,
  startOfMonth,
  endOfMonth,
  eachDayOfInterval,
  isSameMonth,
  isSameDay,
  parseISO
} from "date-fns";
import { sv } from "date-fns/locale";
import gameService from "../../services/gameService";
import teamService from "../../services/teamService";

const HomePage = () => {
  const [games, setGames] = useState([]);
  const [teams, setTeams] = useState({});
  const [selectedDate, setSelectedDate] = useState(new Date());
  const [currentMonth, setCurrentMonth] = useState(new Date());
  const navigate = useNavigate();

  useEffect(() => {
    const fetchGamesAndTeams = async () => {
      try {
        const fetchedGames = await gameService.getAllGames();
        setGames(fetchedGames);

        const teamIds = [
          ...new Set(fetchedGames.flatMap((game) => [game.homeTeamId, game.awayTeamId]))
        ];
        const teamPromises = teamIds.map((teamId) => teamService.getTeamById(teamId));
        const teamData = await Promise.all(teamPromises);

        const teamsMap = teamData.reduce((acc, team) => {
          acc[team.id] = team;
          return acc;
        }, {});
        setTeams(teamsMap);
      } catch (error) {
        console.error("Error fetching games and teams:", error);
      }
    };

    fetchGamesAndTeams();
  }, []);

  const handleGameClick = (game) => {
    const homeTeam = teams[game.homeTeamId] || { name: "Unknown Home Team" };
    const awayTeam = teams[game.awayTeamId] || { name: "Unknown Away Team" };
    navigate(`/game/${game.id}`, {
      state: {
        game,
        homeTeam,
        awayTeam
      }
    });
  };

  const goToPreviousMonth = () => {
    setCurrentMonth((prevMonth) =>
      new Date(prevMonth.getFullYear(), prevMonth.getMonth() - 1)
    );
  };

  const goToNextMonth = () => {
    setCurrentMonth((prevMonth) =>
      new Date(prevMonth.getFullYear(), prevMonth.getMonth() + 1)
    );
  };

  const start = startOfMonth(currentMonth);
  const end = endOfMonth(currentMonth);
  const days = eachDayOfInterval({ start, end });

  const gamesInMonth = games.filter((game) => {
    const gameDate = game.startTime ? parseISO(game.startTime) : null;
    return gameDate && isSameMonth(gameDate, currentMonth);
  });

  const getGameStatus = (game) => {
    if (game.gameStatus === 3) return "#1bff00";
    if (game.gameStatus === 2) return "#f3ff00";
    if (game.gameStatus === 1) return "#ff0000";
    return "#9e9e9e";
  };

  return (
    <div className="flex flex-col items-center min-h-screen p-4 sm:p-8">
      <div className="w-full max-w-4xl bg-gray-50 rounded-lg shadow-md p-4 sm:p-6 mb-8">
        <div className="flex justify-between items-center mb-4">
          <button
            onClick={goToPreviousMonth}
            className="px-4 py-2 bg-gray-200 text-gray-700 rounded hover:bg-gray-300 transition"
          >
            Föregående
          </button>
          <h2 className="text-lg sm:text-2xl font-bold">
            {format(currentMonth, "MMMM yyyy", { locale: sv }).charAt(0).toUpperCase() +
              format(currentMonth, "MMMM yyyy", { locale: sv }).slice(1)}
          </h2>
          <button
            onClick={goToNextMonth}
            className="px-4 py-2 bg-gray-200 text-gray-700 rounded hover:bg-gray-300 transition"
          >
            Nästkommande
          </button>
        </div>

        <div className="grid grid-cols-7 gap-2 sm:gap-4 text-center text-sm sm:text-base font-semibold text-gray-700 mb-2 sm:mb-4">
          {["Mån", "Tis", "Ons", "Tor", "Fre", "Lör", "Sön"].map((day) => (
            <div key={day}>{day}</div>
          ))}
        </div>

        <div className="grid grid-cols-7 gap-2 sm:gap-4">
          {days.map((day) => {
            const dayStr = format(day, "yyyy-MM-dd");
            const gamesOnDay = gamesInMonth.filter((game) =>
              isSameDay(parseISO(game.startTime), day)
            );
            const isSelected = isSameDay(day, selectedDate);

            return (
              <div
                key={`${dayStr}-${gamesOnDay.length > 0 ? "booked" : "no-game"}`}
                className={`border rounded-lg p-2 h-20 text-left text-xs sm:text-sm relative transition cursor-pointer
                  ${!isSameMonth(day, currentMonth) ? "text-gray-400" : ""}
                  ${isSelected ? "bg-red-100 border-red-400" : "hover:bg-red-50"}
                `}
                onClick={() => {
                  setSelectedDate(day);
                  if (gamesOnDay.length > 0) {
                    handleGameClick(gamesOnDay[0]);
                  }
                }}
              >
                <div>{format(day, "d")}</div>
                {gamesOnDay.length > 0 &&
                  gamesOnDay.map((game) => (
                    <span
                      key={game.id}
                      className="absolute bottom-2 left-2 w-2 h-2 rounded-full"
                      style={{ backgroundColor: `${getGameStatus(game)}` }}
                    />
                  ))}
              </div>
            );
          })}
        </div>
      </div>

      <div className="w-full max-w-4xl bg-white rounded-lg shadow-md p-4 sm:p-6">
        <h3 className="text-md sm:text-xl font-bold mb-4">
          Matcher i {format(currentMonth, "MMMM yyyy", { locale: sv })}
        </h3>
        {gamesInMonth.length > 0 ? (
          <ul className="space-y-4">
            {gamesInMonth.map((game) => {
              const homeTeam = teams[game.homeTeamId] || { name: "Unknown Home Team" };
              const awayTeam = teams[game.awayTeamId] || { name: "Unknown Away Team" };

              return (
                <li
                  key={game.id}
                  onClick={() => handleGameClick(game)}
                  className="border rounded p-3 bg-white shadow-sm text-sm md:text-base cursor-pointer hover:bg-gray-100 transition"
                >
                  <p className="font-semibold">
                    {homeTeam.teamName} vs {awayTeam.teamName}
                  </p>
                  <p>
                    {format(parseISO(game.startTime), "yyyy-MM-dd")} Kl.{" "}
                    {format(parseISO(game.startTime), "HH:mm")} -{" "}
                  </p>
                  <p className="text-gray-600">{game.place}</p>
                </li>
              );
            })}
          </ul>
        ) : (
          <p className="text-gray-600 text-sm sm:text-base">Inga matcher planerade.</p>
        )}
      </div>
    </div>
  );
};

export default HomePage;
