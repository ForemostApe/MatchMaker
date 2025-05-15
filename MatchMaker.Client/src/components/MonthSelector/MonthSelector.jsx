import { format } from "date-fns";
import { sv } from "date-fns/locale";

const MonthSelector = ({ currentMonth, setCurrentMonth }) => {
  const goToPreviousMonth = () => {
    setCurrentMonth(
      new Date(currentMonth.getFullYear(), currentMonth.getMonth() - 1)
    );
  };

  const goToNextMonth = () => {
    setCurrentMonth(
      new Date(currentMonth.getFullYear(), currentMonth.getMonth() + 1)
    );
  };

  const formattedMonth =
    format(currentMonth, "MMMM yyyy", { locale: sv }).charAt(0).toUpperCase() +
    format(currentMonth, "MMMM yyyy", { locale: sv }).slice(1);

  return (
    <div className="w-full max-w-4xl bg-gray-50 rounded-lg shadow-md p-4 sm:p-6 mb-8 flex justify-between items-center">
      <button
        onClick={goToPreviousMonth}
        className="px-4 py-2 bg-gray-200 text-gray-700 rounded hover:bg-gray-300 transition cursor-pointer"
      >
        Föregående
      </button>
      <h2 className="text-lg sm:text-2xl font-bold">{formattedMonth}</h2>
      <button
        onClick={goToNextMonth}
        className="px-4 py-2 bg-gray-200 text-gray-700 rounded hover:bg-gray-300 transition cursor-pointer"
      >
        Nästkommande
      </button>
    </div>
  );
};

export default MonthSelector;
