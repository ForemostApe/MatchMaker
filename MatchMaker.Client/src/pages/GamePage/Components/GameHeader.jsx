import { format, parseISO } from "date-fns";
import { sv } from "date-fns/locale";

const GameHeader = ({ homeTeam, awayTeam, game }) => {
  const date = format(parseISO(game.startTime), "EEEE d MMMM", { locale: sv }).replace(/^\w/, c => c.toUpperCase());
  const time = format(parseISO(game.startTime), "HH:mm");

  return (
    <div className="flex flex-row justify-between p-0">
      <div className="w-30 h-30 bg-red-500">Logo</div>
      <div className="flex flex-col text-center justify-between">
        <h1 className="text-xl font-bold">{homeTeam.teamName} vs {awayTeam.teamName}</h1>
        <h2 className="text-xl font-bold">{date}</h2>
        <h2 className="text-xl font-bold">{game.location}, Kl. {time}</h2>
      </div>
      <div className="w-30 h-30 bg-red-500">Logo</div>
    </div>
  );
};

export default GameHeader;