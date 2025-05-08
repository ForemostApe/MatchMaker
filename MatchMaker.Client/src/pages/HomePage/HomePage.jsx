import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { format, startOfMonth, endOfMonth, eachDayOfInterval, isSameMonth, isSameDay, parseISO } from "date-fns";
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

        const teamIds = [...new Set(fetchedGames.map((game) => [game.homeTeamId, game.awayTeamId]).flat())];

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

  const handleGameClick = (gameId) => {
    navigate(`/games/${gameId}`);
  };

  const goToPreviousMonth = () => {
    setCurrentMonth((prevMonth) => new Date(prevMonth.setMonth(prevMonth.getMonth() - 1)));
  };

  const goToNextMonth = () => {
    setCurrentMonth((prevMonth) => new Date(prevMonth.setMonth(prevMonth.getMonth() + 1)));
  };

  const start = startOfMonth(currentMonth);
  const end = endOfMonth(currentMonth);
  const days = eachDayOfInterval({ start, end });

  const gamesInMonth = games.filter((game) => {
    const gameDate = game.startTime ? parseISO(game.startTime) : null;
    return gameDate && isSameMonth(gameDate, currentMonth);
  });

  // Helper function to get the status of the game (Booked, Planned, Cancelled)
  const getGameStatus = (game) => {
    if (game.status === "booked") return "green";
    if (game.status === "planned") return "yellow";
    if (game.status === "cancelled") return "red";
    return "gray"; // default if no status
  };

  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-gray-100 p-4 sm:p-8">
      <div className="w-full max-w-4xl bg-white rounded-lg shadow-md p-4 sm:p-6 mb-8">
        <div className="flex justify-between items-center mb-4">
          <button
            onClick={goToPreviousMonth}
            className="px-4 py-2 bg-gray-200 text-gray-700 rounded hover:bg-gray-300 transition"
          >
            Prev
          </button>
          <h2 className="text-lg sm:text-2xl font-bold">
            {format(currentMonth, "MMMM yyyy")}
          </h2>
          <button
            onClick={goToNextMonth}
            className="px-4 py-2 bg-gray-200 text-gray-700 rounded hover:bg-gray-300 transition"
          >
            Next
          </button>
        </div>

        <div className="grid grid-cols-7 gap-2 sm:gap-4 text-center text-sm sm:text-base font-semibold text-gray-700 mb-2 sm:mb-4">
          {["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"].map((day) => (
            <div key={day}>{day}</div>
          ))}
        </div>

        <div className="grid grid-cols-7 gap-2 sm:gap-4">
          {days.map((day) => {
            const dayStr = format(day, "yyyy-MM-dd");
            const isBooked = gamesInMonth.filter((game) =>
              isSameDay(parseISO(game.startTime), day)
            );
            const isSelected = isSameDay(day, selectedDate);

            return (
              <div
                key={dayStr}
                className={`border rounded-lg p-2 h-20 text-left text-xs sm:text-sm relative transition cursor-pointer
                  ${!isSameMonth(day, currentMonth) ? "text-gray-400" : ""}
                  ${isSelected ? "bg-blue-100 border-blue-400" : "hover:bg-gray-100"}
                `}
                onClick={() => {
                  setSelectedDate(day);
                  if (isBooked.length > 0) {
                    handleGameClick(isBooked[0].id);
                  }
                }}
              >
                <div>{format(day, "d")}</div>
                {isBooked.length > 0 && (
                  <span
                    className={`absolute bottom-1 left-1 w-2 h-2 rounded-full ${getGameStatus(isBooked[0])}`}
                  />
                )}
              </div>
            );
          })}
        </div>
      </div>

      <div className="w-full max-w-4xl bg-white rounded-lg shadow-md p-4 sm:p-6">
        <h3 className="text-md sm:text-xl font-bold mb-4">
          Games in {format(currentMonth, "MMMM yyyy")}
        </h3>
        {gamesInMonth.length > 0 ? (
          <ul className="space-y-4">
            {gamesInMonth.map((game) => {
              const homeTeam = teams[game.homeTeamId] || { name: "Unknown Home Team" };
              const awayTeam = teams[game.awayTeamId] || { name: "Unknown Away Team" };
              return (
                <li
                  key={game.id}
                  onClick={() => handleGameClick(game.id)}
                  className="border rounded p-3 bg-white shadow-sm text-sm md:text-base cursor-pointer hover:bg-gray-100 transition"
                >
                  <p className="font-semibold">
                    {homeTeam.teamName} vs {awayTeam.teamName}
                  </p>
                  <p>
                    {format(parseISO(game.startTime), "yyyy-MM-dd")} Kl.{" "}
                    {format(parseISO(game.startTime), "HH:mm")} -{" "}
                    {format(parseISO(game.endTime), "HH:mm")}
                  </p>
                  <p className="text-gray-600">{game.place}</p>
                </li>
              );
            })}
          </ul>
        ) : (
          <p className="text-gray-600 text-sm sm:text-base">No games this month.</p>
        )}
      </div>
    </div>
  );
};

export default HomePage;
