using System;

class Node
{
    public char key;
    public Node left, right, parent;
    public int color; // 0 for black, 1 for red

    public Node(char item)
    {
        key = item;
        left = right = parent = null;
        color = 1; 
    }
}

class RedBlackTree
{
    private Node root;
    private Node TNULL;

    public RedBlackTree()
    {
        TNULL = new Node('0'); // Sentinel node
        TNULL.color = 0; 
        root = TNULL;
    }

    // Rotation and Insertion Fix Methods
    private void LeftRotate(Node x)
    {
        Node y = x.right;
        x.right = y.left;
        if (y.left != TNULL)
        {
            y.left.parent = x;
        }
        y.parent = x.parent;
        if (x.parent == null)
        {
            root = y;
        }
        else if (x == x.parent.left)
        {
            x.parent.left = y;
        }
        else
        {
            x.parent.right = y;
        }
        y.left = x;
        x.parent = y;
    }

    private void RightRotate(Node x)
    {
        Node y = x.left;
        x.left = y.right;
        if (y.right != TNULL)
        {
            y.right.parent = x;
        }
        y.parent = x.parent;
        if (x.parent == null)
        {
            root = y;
        }
        else if (x == x.parent.right)
        {
            x.parent.right = y;
        }
        else
        {
            x.parent.left = y;
        }
        y.right = x;
        x.parent = y;
    }

    private void FixInsert(Node k)
    {
        Node u;
        while (k.parent.color == 1)
        {
            if (k.parent == k.parent.parent.right)
            {
                u = k.parent.parent.left;
                if (u.color == 1)
                {
                    u.color = 0;
                    k.parent.color = 0;
                    k.parent.parent.color = 1;
                    k = k.parent.parent;
                }
                else
                {
                    if (k == k.parent.left)
                    {
                        k = k.parent;
                        RightRotate(k);
                    }
                    k.parent.color = 0;
                    k.parent.parent.color = 1;
                    LeftRotate(k.parent.parent);
                }
            }
            else
            {
                u = k.parent.parent.right;
                if (u.color == 1)
                {
                    u.color = 0;
                    k.parent.color = 0;
                    k.parent.parent.color = 1;
                    k = k.parent.parent;
                }
                else
                {
                    if (k == k.parent.right)
                    {
                        k = k.parent;
                        LeftRotate(k);
                    }
                    k.parent.color = 0;
                    k.parent.parent.color = 1;
                    RightRotate(k.parent.parent);
                }
            }
            if (k == root)
            {
                break;
            }
        }
        root.color = 0;
    }

    public void Insert(char key)
    {
        Node node = new Node(key);
        node.parent = null;
        node.left = TNULL;
        node.right = TNULL;

        Node y = null;
        Node x = this.root;

        while (x != TNULL)
        {
            y = x;
            if (node.key < x.key)
            {
                x = x.left;
            }
            else if (node.key > x.key) // prevent duplicate
            {
                x = x.right;
            }
            else
            {
                Console.WriteLine("Duplicate key not allowed.");
                return;
            }
        }

        node.parent = y;
        if (y == null)
        {
            root = node;
        }
        else if (node.key < y.key)
        {
            y.left = node;
        }
        else
        {
            y.right = node;
        }

        if (node.parent == null)
        {
            node.color = 0;
            return;
        }

        if (node.parent.parent == null)
        {
            return;
        }

        FixInsert(node);
    }

    // Deletion Methods
    private void Transplant(Node u, Node v)
    {
        if (u.parent == null)
            root = v;
        else if (u == u.parent.left)
            u.parent.left = v;
        else
            u.parent.right = v;
        v.parent = u.parent;
    }

    private Node Minimum(Node node)
    {
        while (node.left != TNULL)
            node = node.left;
        return node;
    }

    public void Delete(char key)
    {
        Node node = root;
        Node z = TNULL, x, y;

        while (node != TNULL)
        {
            if (node.key == key)
            {
                z = node;
            }

            if (node.key <= key)
            {
                node = node.right;
            }
            else
            {
                node = node.left;
            }
        }

        if (z == TNULL)
        {
            Console.WriteLine("Key not found in the tree.");
            return;
        }

        y = z;
        int yOriginalColor = y.color;
        if (z.left == TNULL)
        {
            x = z.right;
            Transplant(z, z.right);
        }
        else if (z.right == TNULL)
        {
            x = z.left;
            Transplant(z, z.left);
        }
        else
        {
            y = Minimum(z.right);
            yOriginalColor = y.color;
            x = y.right;
            if (y.parent == z)
            {
                x.parent = y;
            }
            else
            {
                Transplant(y, y.right);
                y.right = z.right;
                y.right.parent = y;
            }

            Transplant(z, y);
            y.left = z.left;
            y.left.parent = y;
            y.color = z.color;
        }

        if (yOriginalColor == 0)
        {
            FixDelete(x);
        }
    }

    private void FixDelete(Node x)
    {
        Node s;
        while (x != root && x.color == 0)
        {
            if (x == x.parent.left)
            {
                s = x.parent.right;
                if (s.color == 1)
                {
                    s.color = 0;
                    x.parent.color = 1;
                    LeftRotate(x.parent);
                    s = x.parent.right;
                }

                if (s.left.color == 0 && s.right.color == 0)
                {
                    s.color = 1;
                    x = x.parent;
                }
                else
                {
                    if (s.right.color == 0)
                    {
                        s.left.color = 0;
                        s.color = 1;
                        RightRotate(s);
                        s = x.parent.right;
                    }

                    s.color = x.parent.color;
                    x.parent.color = 0;
                    s.right.color = 0;
                    LeftRotate(x.parent);
                    x = root;
                }
            }
            else
            {
                s = x.parent.left;
                if (s.color == 1)
                {
                    s.color = 0;
                    x.parent.color = 1;
                    RightRotate(x.parent);
                    s = x.parent.left;
                }

                if (s.left.color == 0 && s.right.color == 0)
                {
                    s.color = 1;
                    x = x.parent;
                }
                else
                {
                    if (s.left.color == 0)
                    {
                        s.right.color = 0;
                        s.color = 1;
                        LeftRotate(s);
                        s = x.parent.left;
                    }

                    s.color = x.parent.color;
                    x.parent.color = 0;
                    s.left.color = 0;
                    RightRotate(x.parent);
                    x = root;
                }
            }
        }
        x.color = 0;
    }

    // Utility Methods for Tree Traversal
    public void InOrder()
    {
        InOrderHelper(root);
        Console.WriteLine();
    }

    private void InOrderHelper(Node node)
    {
        if (node != TNULL)
        {
            InOrderHelper(node.left);
            Console.Write(node.key + " ");
            InOrderHelper(node.right);
        }
    }

    public void PreOrder()
    {
        PreOrderHelper(root);
        Console.WriteLine();
    }

    private void PreOrderHelper(Node node)
    {
        if (node != TNULL)
        {
            Console.Write(node.key + " ");
            PreOrderHelper(node.left);
            PreOrderHelper(node.right);
        }
    }

    public void PostOrder()
    {
        PostOrderHelper(root);
        Console.WriteLine();
    }

    private void PostOrderHelper(Node node)
    {
        if (node != TNULL)
        {
            PostOrderHelper(node.left);
            PostOrderHelper(node.right);
            Console.Write(node.key + " ");
        }
    }

    public int Height()
    {
        return HeightHelper(root);
    }

    private int HeightHelper(Node node)
    {
        if (node == TNULL) return -1;
        return Math.Max(HeightHelper(node.left), HeightHelper(node.right)) + 1;
    }

    public void PrintTree()
    {
        if (root == TNULL)
        {
            Console.WriteLine("(Empty tree)");
            return;
        }
        
        PrintNodeInternal(root, "", true);
    }

    private void PrintNodeInternal(Node node, string indent, bool isRight)
    {
        if (node != TNULL)
        {
            Console.Write(indent);

            if (isRight)
            {
                Console.Write("R---- ");
                indent += "     ";
            }
            else
            {
                Console.Write("L---- ");
                indent += "|    ";
            }

            // Print color-coded node
            if (node.color == 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{node.key}");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"{node.key}");
            }

            PrintNodeInternal(node.left, indent, false);
            PrintNodeInternal(node.right, indent, true);
        }
    }

    public static void Main(string[] args)
    {
        RedBlackTree tree = new RedBlackTree();
        while (true)
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. Insert");
            Console.WriteLine("2. Delete");
            Console.WriteLine("3. In-Order Traversal");
            Console.WriteLine("4. Pre-Order Traversal");
            Console.WriteLine("5. Post-Order Traversal");
            Console.WriteLine("6. Print Tree Structure");
            Console.WriteLine("7. Height of Tree");
            Console.WriteLine("8. Exit");
            Console.Write("Choose an option: ");
            char choice = Console.ReadKey().KeyChar;
            Console.WriteLine();

            switch (choice)
            {
                case '1':
                    Console.Write("Enter character to insert: ");
                    char insertChar = Console.ReadKey().KeyChar;
                    tree.Insert(insertChar);
                    break;
                case '2':
                    Console.Write("Enter character to delete: ");
                    char deleteChar = Console.ReadKey().KeyChar;
                    tree.Delete(deleteChar);
                    break;
                case '3':
                    Console.WriteLine("In-Order Traversal:");
                    tree.InOrder();
                    break;
                case '4':
                    Console.WriteLine("Pre-Order Traversal:");
                    tree.PreOrder();
                    break;
                case '5':
                    Console.WriteLine("Post-Order Traversal:");
                    tree.PostOrder();
                    break;
                case '6': 
                    Console.WriteLine("Tree structure:");
                    tree.PrintTree();
                    break;
                case '7':
                    Console.WriteLine("Height of Tree: " + tree.Height());
                    break;
                case '8':
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
}
