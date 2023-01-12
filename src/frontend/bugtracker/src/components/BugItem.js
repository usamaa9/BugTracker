const BugItem = ({ bug }) => {
  const getDate = (dateString) => {
    if (dateString === null) return null;
    const date = new Date(dateString);
    const formattedDate = date.toLocaleDateString('en-GB', {
      day: 'numeric',
      month: 'numeric',
      year: 'numeric',
    });
    return formattedDate;
  };

  return (
    <div className="card col-6">
      <div className="card-header d-flex justify-content-between align-items-center">
        <h3 className="p-2 flex-grow-1">{bug.title}</h3>
        <button type="button" className="btn btn-outline-primary">
          Assign
        </button>
        {bug.isOpen ? (
          <button type="button" className="btn btn-outline-warning" style={{ color: 'black' }}>
            Close
          </button>
        ) : null}
      </div>
      <div className="card-body">
        <p className="card-text">Description: {bug.description}</p>
        <p className="card-text">Date opened: {getDate(bug.dateOpened)}</p>
        {bug.isOpen ? null : <p className="card-text">Date closed: {getDate(bug.dateClosed)}</p>}
        {bug.assignedTo == null ? null : <p className="card-text">Assigned to: {bug.assignedTo.name}</p>}
      </div>
    </div>
  );
};

export default BugItem;
