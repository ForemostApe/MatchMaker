import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import gameService from "../../services/gameService";
import teamService from "../../services/teamService";
import { parseISO, isSameMonth } from "date-fns";
import { useAuth } from "../../context/AuthContext/AuthContext";
import Calendar from "../../components/Calendar/Calendar";
import GameList from "../../components/GameList/GameList";
import MonthSelector from "../../components/MonthSelector/MonthSelector";
import PendingGamesList from "../../components/PendingGamesList/PendingGamesList";

const HomePage = () => {
  const { user } = useAuth();
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
        const teamData = await Promise.all(teamIds.map((id) => teamService.getTeamById(id)));

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
    const homeTeam = teams[game.homeTeamId] || { name: "Okänt hemmalag" };
    const awayTeam = teams[game.awayTeamId] || { name: "Okänt bortalag" };
    navigate(`/game/${game.id}`, {
      state: { game, homeTeam, awayTeam }
    });
  };

  const gamesInMonth = games.filter(game => {
    const date = game.startTime ? parseISO(game.startTime) : null;
    return date && isSameMonth(date, currentMonth);
  });

  return (
    <div className="flex flex-col items-center min-h-screen p-4 sm:p-8">
      <MonthSelector
        currentMonth={currentMonth}
        setCurrentMonth={setCurrentMonth}
      />

      <Calendar
        currentMonth={currentMonth}
        selectedDate={selectedDate}
        setSelectedDate={setSelectedDate}
        games={gamesInMonth}
        onGameClick={handleGameClick}
      />

      <PendingGamesList
        games={games}
        teams={teams}
        user={user}
        onGameClick={handleGameClick}
      />

      <GameList
        games={gamesInMonth}
        teams={teams}
        onGameClick={handleGameClick}
        currentMonth={currentMonth}
      />
    </div>
  );
};

export default HomePage;
