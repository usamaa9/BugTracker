import React from 'react';

const BugItemDetails = ({ bug }) => {
  return (
    <div>
      <p>{bug.description}</p>
      <p>{bug.dateOpen}</p>
      <p>Open?: {bug.isOpen ? 'Yes' : 'No'}</p>
    </div>
  );
};

export default BugItemDetails;
