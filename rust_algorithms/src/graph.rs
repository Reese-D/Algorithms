use crate::union_set;

#[derive(Debug)]
pub struct GraphEdge {
    pub from: i32,
    pub to: i32,
    pub weight: i32
}

impl GraphEdge {
    pub fn new(from: i32, to: i32, weight: i32) -> Self {
	GraphEdge {
	    from,
	    to,
	    weight
	}
    }
}

#[derive(Debug)]
pub struct Graph<T: PartialEq> {
    nodes: Vec<T>,
    edges: Vec<GraphEdge>
}


//Assumes it's undirected, in this case "from" and "to" are more like node A and B, bidirectional.
pub fn kruskal_minimum_spanning_tree<T: PartialEq>(graph: &mut Graph<T>) -> Vec<GraphEdge>{
    let mut set = union_set::build_union_set(graph.nodes.len() as i32);
    let mut result: Vec<GraphEdge> = vec![];
    
    //make sure we have the smallest edges possible first
    graph.edges.sort_by(|a,b| a.weight.cmp(&b.weight));
    for edge in &graph.edges {
	let from = union_set::find(&set, edge.from);
	let to = union_set::find(&set, edge.to);
	if (from == -1 && to == -1) || from != to {
	    union_set::merge(&mut set, from, to);
	    result.push(GraphEdge::new(edge.from, edge.to, edge.weight));
	}
    }

    return result;
}


#[cfg(test)]
mod test {
    use super::*;
    
    #[test]
    fn kruskal_minimum_spanning_tree_test(){
	let mut my_graph = Graph {
	    nodes: vec![1,5,2,3,4,9],
	    edges: vec![GraphEdge::new(0,1,2)
			,GraphEdge::new(0,2,3)
			,GraphEdge::new(3,4,1)
			,GraphEdge::new(4,5,5)
			,GraphEdge::new(2,4,1)
			,GraphEdge::new(1,5,10)
			,GraphEdge::new(3,1,2)
			,GraphEdge::new(5,3,1)
	    ]
	};

	let result = kruskal_minimum_spanning_tree(&mut my_graph);
	println!("{0:#?}", result);
	assert!(result.iter().map(|x| x.weight).sum::<i32>() == 7);
	assert!(result.len() == 5);
    }
	
}
