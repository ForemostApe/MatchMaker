import { useState } from "react";

export default function GameEdit({ game }) {
  const [formData, setFormData] = useState({ ...game });

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSave = () => {
    // Call API to save
  };

  return (
    <div className="space-y-2">
      <input
        name="homeTeam"
        value={formData.homeTeam}
        onChange={handleChange}
        className="border p-2 w-full"
      />
      <input
        name="awayTeam"
        value={formData.awayTeam}
        onChange={handleChange}
        className="border p-2 w-full"
      />
      
      <button onClick={handleSave} className="mt-4 bg-blue-500 text-white px-4 py-2 rounded">
        Save Changes
      </button>
    </div>
  );
}
