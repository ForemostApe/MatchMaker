const ConditionItem = ({ title, value, editable, onChange, field }) => (
  <div className="m-1 mt-5">
    <h3 className="font-bold">{title}</h3>
    {editable ? (
      <textarea
        className="w-full border rounded p-2"
        value={value}
        onChange={(e) => onChange(field, e.target.value)}
      />
    ) : (
      <div>{value}</div>
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
    <div>
      <h2 className="text-lg font-semibold mt-6 mb-2">Matchvillkor</h2>
      <ConditionItem title="Spelplan:" value={c.court} editable={canEdit && editing} onChange={handleChange} field="court" />
      <ConditionItem title="Offensiva överenskommelser:" value={c.offensiveConditions} editable={canEdit && editing} onChange={handleChange} field="offensiveConditions" />
      <ConditionItem title="Defensiva överenskommelser:" value={c.defensiveConditions} editable={canEdit && editing} onChange={handleChange} field="defensiveConditions" />
      <ConditionItem title="Specialister:" value={c.specialists} editable={canEdit && editing} onChange={handleChange} field="specialists" />
      <ConditionItem title="Bestraffningar:" value={c.penalties} editable={canEdit && editing} onChange={handleChange} field="penalties" />
    </div>
  );
};

export default GameConditions;
