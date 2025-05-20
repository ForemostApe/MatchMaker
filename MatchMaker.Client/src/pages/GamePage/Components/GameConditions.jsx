const ConditionItem = ({ title, value, editable, onChange, field }) => (
  <div className="mb-4">
    <label className="block text-gray-700 mb-2 font-bold">{title}</label>
    {editable ? (
      <textarea
        className="w-full px-3 py-2 border rounded-md"
        value={value}
        onChange={(e) => onChange(field, e.target.value)}
        rows={3}
      />
    ) : (
      <p className="text-gray-900 whitespace-pre-wrap">{value}</p>
    )}
  </div>
);

const GameConditions = ({ editing, canEdit, formState, setFormState }) => {
  const handleChange = (field, value) => {
    setFormState(prev => ({
      ...prev,
      conditions: {
        ...prev.conditions,
        [field]: value
      }
    }));
  };

  const c = formState.conditions;

  return (
    <div className="max-w-md mx-auto p-6">
      <h2 className="text-2xl font-bold mb-4 text-gray-800">Matchvillkor</h2>

      <ConditionItem
        title="Plan:"
        value={c.court}
        editable={canEdit && editing}
        onChange={handleChange}
        field="court"
      />
      <ConditionItem
        title="Tidhållning:"
        value={c.timing}
        editable={canEdit && editing}
        onChange={handleChange}
        field="timing"
      />
      <ConditionItem
        title="Offensiva överenskommelser:"
        value={c.offensiveConditions}
        editable={canEdit && editing}
        onChange={handleChange}
        field="offensiveConditions"
      />
      <ConditionItem
        title="Defensiva överenskommelser:"
        value={c.defensiveConditions}
        editable={canEdit && editing}
        onChange={handleChange}
        field="defensiveConditions"
      />
      <ConditionItem
        title="Specialister:"
        value={c.specialists}
        editable={canEdit && editing}
        onChange={handleChange}
        field="specialists"
      />
      <ConditionItem
        title="Bestraffningar:"
        value={c.penalties}
        editable={canEdit && editing}
        onChange={handleChange}
        field="penalties"
      />
    </div>
  );
};

export default GameConditions;