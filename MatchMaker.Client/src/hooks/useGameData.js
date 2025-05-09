import { useState, useEffect } from "react";
import gameService from "../services/gameService";
import teamService from "../services/teamService";

const useGameData = (id, location) => {
  const [game, setGame] = useState(location.state?.game || null);
  const [homeTeam, setHomeTeam] = useState(location.state?.homeTeam || null);
  const [awayTeam, setAwayTeam] = useState(location.state?.awayTeam || null);
  const [loading, setLoading] = useState(!location.state);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (location.state?.game && location.state?.homeTeam && location.state?.awayTeam) {
      return;
    }

    const fetchData = async () => {
      try {
        const fetchedGame = await gameService.getGameById(id);
        setGame(fetchedGame);
        const fetchedHome = await teamService.getTeamById(fetchedGame.homeTeamId);
        const fetchedAway = await teamService.getTeamById(fetchedGame.awayTeamId);
        setHomeTeam(fetchedHome);
        setAwayTeam(fetchedAway);
      } catch (err) {
        console.error("Failed to fetch game or teams", err);
        setError(err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id, location.state]);

  return { game, homeTeam, awayTeam, loading, error };
};

export default useGameData;
