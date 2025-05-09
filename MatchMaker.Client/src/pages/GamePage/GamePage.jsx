import { useParams, useLocation } from "react-router-dom";
import useGameData from "../../hooks/useGameData";
import { format, parseISO } from "date-fns";
import { sv } from "date-fns/locale";

const GamePage = () => {
  const { id } = useParams();
  const location = useLocation();
  const { game, homeTeam, awayTeam, loading, error } = useGameData(id, location);

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error loading game data.</p>;

  return (
    <div className="p-4 max-w-3xl mx-auto bg-white shadow rounded">
      <div className="flex flex-row justify justify-between p-0">
        <div className="w-30 h-30 bg-red-500">
          Logo
        </div>
        <div className="flex flex-col text-center justify-between border-solid h-auto">
            <div className="border-solid"><h1 className="text-xl font-bold">{homeTeam.teamName} vs {awayTeam.teamName}</h1></div>
            <div className="border-solid"><h2 className="text-xl font-bold">{format(parseISO(game.startTime), "EEEE d MMMM", { locale: sv }).replace(/^\w/, c => c.toUpperCase())}</h2></div>
            <div className="border-solid"><h2 className="text-xl font-bold">{game.location}, Kl. {format(parseISO(game.startTime), "HH:mm")}</h2></div>
        </div>
        <div className="w-30 h-30 bg-red-500">
          Logo
        </div>
      </div>
      <div>
        <div className="m-1 mt-5">
          <span><h3 className="font-bold">Spelplan:</h3></span> 
          <div>
            {game.conditions.court}
          </div>
        </div>
        <div className="m-1 mt-5">
          <span><h3 className="font-bold">Offensiva överrenskommelser:</h3></span> 
          <div>
            {game.conditions.offensiveConditions}
          </div>
        </div>
        <div className="m-1 mt-5">
          <span><h3 className="font-bold">Defensiva överrensskommelser:</h3></span> 
          <div>
            {game.conditions.defensiveConditions}
          </div>
        </div>
        <div className="m-1 mt-5">
          <span><h3 className="font-bold">Specialister:</h3></span> 
            <div>
              {game.conditions.specialists}
            </div>
          </div>
        <div className="m-1 mt-5">
          <span><h3 className="font-bold">Bestraffningar:</h3></span>
            <div>
              {game.conditions.penalties}
            </div>
          </div>
      </div>
    </div>
  );
};

export default GamePage;
