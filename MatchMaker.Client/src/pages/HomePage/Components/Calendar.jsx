import { format, startOfMonth, endOfMonth, eachDayOfInterval, isSameMonth, isSameDay, parseISO } from "date-fns";

const Calendar = ({ currentMonth, selectedDate, setSelectedDate, games, onGameClick }) => {
  const start = startOfMonth(currentMonth);
  const end = endOfMonth(currentMonth);
  const days = eachDayOfInterval({ start, end });

  const getGameStatusColor = (game) => {
    switch (game.gameStatus) {
      case 3: return "#1bff00";
      case 2: return "#f3ff00";
      case 1: return "#ff0000";
      default: return "#9e9e9e";
    }
  };

  return (
    <div className="w-full max-w-4xl bg-gray-50 rounded-lg shadow-md p-4 sm:p-6 mb-5">
      <div className="grid grid-cols-7 gap-2 sm:gap-4 text-center text-sm sm:text-base font-semibold text-gray-700 mb-2 sm:mb-4">
        {["Mån", "Tis", "Ons", "Tor", "Fre", "Lör", "Sön"].map((day) => (
          <div key={day}>{day}</div>
        ))}
      </div>

      <div className="grid grid-cols-7 gap-2 sm:gap-4">
        {days.map((day) => {
          const dayStr = format(day, "yyyy-MM-dd");
          const gamesOnDay = games.filter((game) =>
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
                  onGameClick(gamesOnDay[0]);
                }
              }}
            >
              <div>{format(day, "d")}</div>
              {gamesOnDay.map((game) => (
                <span
                  key={game.id}
                  className="absolute bottom-2 left-2 w-2 h-2 rounded-full"
                  style={{ backgroundColor: getGameStatusColor(game) }}
                />
              ))}
            </div>
          );
        })}
      </div>
    </div>
  );
};

export default Calendar;
