import React, { useEffect, useState } from "react";
import {
  startOfMonth,
  endOfMonth,
  eachDayOfInterval,
  format,
  isSameMonth,
  isSameDay,
  parseISO,
} from "date-fns";
import gameService from "../../services/gameService";
import { useNavigate } from "react-router-dom";

const HomePage = () => {
  const [bookings, setBookings] = useState([]);
  const [selectedDate, setSelectedDate] = useState(new Date());
  const [currentMonth, setCurrentMonth] = useState(new Date());
  const navigate = useNavigate();

  useEffect(() => {
    const fetchBookings = async () => {
      try {
        const games = await gameService.getAllGames();
        console.log("Fetched games:", games); // Ensure all games are fetched
        setBookings(games);
      } catch (error) {
        console.error("Error fetching bookings:", error);
      }
    };
  
    fetchBookings();
  }, []);

  const handleBookingClick = (bookingId) => {
    navigate(`/games/${bookingId}`);
  };

  const start = startOfMonth(currentMonth);
  const end = endOfMonth(currentMonth);
  const days = eachDayOfInterval({ start, end });

  const bookingsInMonth = bookings.filter(
    (booking) => booking.date && isSameMonth(parseISO(booking.date), currentMonth)
  );

  const goToPreviousMonth = () => {
    setCurrentMonth((prevMonth) => {
      const newDate = new Date(prevMonth);
      newDate.setMonth(prevMonth.getMonth() - 1);
      return newDate;
    });
  };

  const goToNextMonth = () => {
    setCurrentMonth((prevMonth) => {
      const newDate = new Date(prevMonth);
      newDate.setMonth(prevMonth.getMonth() + 1);
      return newDate;
    });
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
            const isBooked = bookingsInMonth.filter(
              (booking) =>
                booking.date && isSameDay(parseISO(booking.date), day)
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
                    handleBookingClick(isBooked[0].id);
                  }
                }}
              >
                <div>{format(day, "d")}</div>
                {isBooked.length > 0 && (
                  <span className="absolute bottom-1 left-1 w-2 h-2 bg-red-500 rounded-full" />
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
        {bookingsInMonth.length > 0 ? (
          <ul className="space-y-4">
            {bookingsInMonth.map((booking) => (
              <li
                key={booking.id}
                onClick={() => handleBookingClick(booking.id)}
                className="border rounded p-3 bg-white shadow-sm text-sm md:text-base cursor-pointer hover:bg-gray-100 transition"
              >
                <p className="font-semibold">
                  {booking.homeTeam} vs {booking.awayTeam}
                </p>
                <p>
                  {booking.date
                    ? `${format(parseISO(booking.date), "yyyy-MM-dd")} at ${
                        booking.time || "unknown time"
                      }`
                    : "Unknown date"}
                </p>
                <p className="text-gray-600">{booking.place || "Unknown place"}</p>
              </li>
            ))}
          </ul>
        ) : (
          <p className="text-gray-600 text-sm sm:text-base">No games this month.</p>
        )}
      </div>
    </div>
  );
};

export default HomePage;
