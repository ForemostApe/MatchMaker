import React, { useState } from "react";
import {
  format,
  startOfMonth,
  endOfMonth,
  startOfWeek,
  endOfWeek,
  addDays,
  addMonths,
  subMonths,
  isSameMonth,
  isSameDay,
  parseISO,
} from "date-fns";

const mockBookings = [
  {
    id: 1,
    date: "2025-04-15",
    homeTeam: "Beckomberga Maniacs",
    awayTeam: "Ösmo Breadwinners",
    time: "18:00",
    place: "Globen Force Dome",
  },
  {
    id: 2,
    date: "2025-04-21",
    homeTeam: "Salem Laserbadgers",
    awayTeam: "Tungelsta Loverboys",
    time: "20:00",
    place: "Kungsängen Grillad Kyckling Arena",
  },
  {
    id: 3,
    date: "2025-05-01",
    homeTeam: "Salem Laserbadgers",
    awayTeam: "Tungelsta Loverboys",
    time: "17:00",
    place: "Kungsängen Grillad Kyckling Arena",
  },
];

export default function HomePage() {
  const [currentMonth, setCurrentMonth] = useState(new Date());
  const [selectedDate, setSelectedDate] = useState(null);

  const monthStart = startOfMonth(currentMonth);
  const monthEnd = endOfMonth(monthStart);
  const startDate = startOfWeek(monthStart, { weekStartsOn: 1 });
  const endDate = endOfWeek(monthEnd, { weekStartsOn: 1 });

  const nextMonth = () => setCurrentMonth(addMonths(currentMonth, 1));
  const prevMonth = () => setCurrentMonth(subMonths(currentMonth, 1));

  const generateDates = () => {
    const dates = [];
    let day = startDate;

    while (day <= endDate) {
      dates.push(day);
      day = addDays(day, 1);
    }
    return dates;
  };

  const bookingsInMonth = mockBookings.filter(
    (b) => format(parseISO(b.date), "yyyy-MM") === format(currentMonth, "yyyy-MM")
  );

  const bookingsByDay = bookingsInMonth.reduce((acc, booking) => {
    acc[booking.date] = acc[booking.date] || [];
    acc[booking.date].push(booking);
    return acc;
  }, {});

  return (
    <div className="container mx-auto px-4 sm:px-6 lg:px-8 py-6">
      <h1 className="text-2xl font-bold mb-4">Bokningskalender</h1>

      <div className="flex items-center justify-between mb-4">
        <button
          onClick={prevMonth}
          className="px-3 py-1 bg-gray-200 rounded hover:bg-gray-300"
        >
          ←
        </button>
        <h2 className="text-lg font-semibold">
          {format(currentMonth, "MMMM yyyy")}
        </h2>
        <button
          onClick={nextMonth}
          className="px-3 py-1 bg-gray-200 rounded hover:bg-gray-300"
        >
          →
        </button>
      </div>

      <div className="overflow-x-auto">
        <div className="grid grid-cols-7 min-w-[560px] gap-2 text-center text-sm font-medium mb-2">
          {["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"].map((day) => (
            <div key={day} className="text-gray-600">
              {day}
            </div>
          ))}
        </div>

        <div className="grid grid-cols-7 min-w-[560px] gap-2 text-center">
          {generateDates().map((day) => {
            const dayStr = format(day, "yyyy-MM-dd");
            const isBooked = bookingsByDay[dayStr];
            const isSelected = selectedDate && isSameDay(day, selectedDate);

            return (
              <button
                key={dayStr}
                onClick={() => setSelectedDate(day)}
                className={`border rounded-lg p-2 h-20 text-left text-xs sm:text-sm relative transition
                  ${!isSameMonth(day, currentMonth) ? "text-gray-400" : ""}
                  ${isSelected ? "bg-blue-100 border-blue-400" : "hover:bg-gray-100"}
                `}
              >
                <div>{format(day, "d")}</div>
                {isBooked && (
                  <span className="absolute bottom-1 left-1 w-2 h-2 bg-red-500 rounded-full" />
                )}
              </button>
            );
          })}
        </div>
      </div>

      <div className="mt-6">
        <h3 className="text-lg font-semibold mb-2">Månadens matcher</h3>
        {bookingsInMonth.length === 0 ? (
          <p className="text-gray-500">Inga bokningar har gjorts.</p>
        ) : (
          <ul className="space-y-3">
            {bookingsInMonth.map((booking) => (
              <li
                key={booking.id}
                className="border rounded p-3 bg-white shadow-sm text-sm md:text-base"
              >
                <p className="font-semibold">
                  {booking.homeTeam} vs {booking.awayTeam}
                </p>
                <p>
                  {booking.date} at {booking.time}
                </p>
                <p className="text-gray-600">{booking.place}</p>
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  );
}
