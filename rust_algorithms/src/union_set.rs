//Note this data structure is one indexed, zero indicates no parent.
//It can be visualized as a backwards tree. if parent_index is [0, 1] then node 2 has node 1 as a parent.
//and node 1 has no parents, and is a root.

#[derive(Debug)]
pub struct UnionSet {
    pub parent_index: Vec<i32>,
    pub subtree_size: Vec<i32>,
    pub count: i32,
}

pub fn build_union_set(item_count: i32) -> UnionSet {
    return UnionSet {
        parent_index: vec![-1; item_count as usize],
        subtree_size: vec![1; item_count as usize],
        count: 7,
    };
}

pub fn merge(set: &mut UnionSet, first: i32, second: i32) {
    let parent_first = find(set, first) as usize;
    let parent_second = find(set, second) as usize;
    if set.subtree_size[parent_first] >= set.subtree_size[parent_second] {
        set.subtree_size[parent_first] =
            set.subtree_size[parent_first] + set.subtree_size[parent_second];
        set.parent_index[parent_second] = first;
    } else {
        set.subtree_size[parent_second] =
            set.subtree_size[parent_second] + set.subtree_size[parent_first];
        set.parent_index[parent_first] = second;
    }
}

pub fn find(set: &UnionSet, index: i32) -> i32 {
    let mut parent: i32 = set.parent_index[index as usize];
    let mut result = index;
    while parent != -1 {
        result = parent;
        parent = set.parent_index[result as usize];
    }
    return result;
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_find() {
        let example = UnionSet {
            parent_index: vec![1, -1, 1, 2, -1, 4, -1],
            subtree_size: vec![0, 3, 1, 0, 1, 0, 0],
            count: 7,
        };

        assert!(find(&example, 0) == 1);
        assert!(find(&example, 1) == 1);
        assert!(find(&example, 2) == 1);
        assert!(find(&example, 3) == 1);
        assert!(find(&example, 4) == 4);
        assert!(find(&example, 5) == 4);
        assert!(find(&example, 6) == 6);
    }

    #[test]
    fn test_merge() {
        let mut example = build_union_set(7);
        println!("{0:#?}", example.parent_index);
        merge(&mut example, 1, 0);
        println!("{0:#?}", example.parent_index);
        merge(&mut example, 2, 3);
        println!("{0:#?}", example.parent_index);
        merge(&mut example, 1, 3);
        println!("{0:#?}", example.parent_index);
        merge(&mut example, 4, 5);
        println!("{0:#?}", example.parent_index);
        assert!(example.parent_index == vec![1, -1, 1, 2, -1, 4, -1]);
        assert!(example.subtree_size == vec![1, 4, 2, 1, 2, 1, 1]);
    }
}
