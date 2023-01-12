import BugItem from './BugItem'; // import the BugItem component
import Col from 'react-bootstrap/Col';
import ListGroup from 'react-bootstrap/ListGroup';
import Row from 'react-bootstrap/Row';
import Tab from 'react-bootstrap/Tab';

const BugList = ({ bugs }) => {
  return (
    <Tab.Container id="list-group-tabs-example" defaultActiveKey="#link1">
      <Row>
        <Col sm={4}>
          <ListGroup>
            {bugs.map((bug) => (
              <ListGroup.Item action href={'#' + bug.id}>
                {bug.title}
              </ListGroup.Item>
            ))}
          </ListGroup>
        </Col>
        <Col sm={8}>
          <Tab.Content>
            {bugs.map((bug) => (
              <Tab.Pane eventKey={'#' + bug.id}>{<BugItem bug={bug} />}</Tab.Pane>
            ))}
          </Tab.Content>
        </Col>
      </Row>
    </Tab.Container>
  );
};

export default BugList;
