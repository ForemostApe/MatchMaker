import { format, startOfMonth, endOfMonth, eachDayOfInterval, isSameMonth, isSameDay, parseISO } from "date-fns";

const Calendar = ({ currentMonth, selectedDate, setSelectedDate, games, onGameClick }) => {
  const start = startOfMonth(currentMonth);
  const end = endOfMonth(currentMonth);
  const days = eachDayOfInterval({ start, end });

  const getGameStatusColor = (game) => {
    switch (game.gameStatus) {
      case "Booked": return "#00b000";
      case "Signed": return "#e6e600";
      case "Cancelled": return "#ff0000";
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
                {gamesOnDay.length > 0 && (
                  <div className="absolute bottom-2 left-2 right-2 flex flex-wrap gap-1">
                    {gamesOnDay.slice(0, 5).map((game) => (
                      <span
                        key={game.id}
                        className="w-3 h-3 rounded-full shadow"
                        style={{ backgroundColor: getGameStatusColor(game) }}
                        title={game.title || "Game"}
                      />
                    ))}
                    {gamesOnDay.length > 5 && (
                      <span className="text-[10px] text-gray-500 ml-1">+{gamesOnDay.length - 5}</span>
                    )}
                  </div>
                )}
            </div>
          );
        })}
      </div>
    </div>
  );
};

export default Calendar;
